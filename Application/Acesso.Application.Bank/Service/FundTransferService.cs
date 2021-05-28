using Acesso.Application.Bank.ViewModel;
using RabbitMQ.Client;
using System;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Acesso.Application.Bank.Interface;
using RabbitMQ.Client.Events;
using Acesso.Domain.Bank.Interfaces;
using Acesso.Domain.Bank.Models;

namespace Acesso.Application.Bank.Service {
    public class FundTransferService : IFundTransferService {
        
        private readonly ConnectionFactory _factory;
        private readonly IAccountHttpClientService _accountHttpClientService;
        private readonly IFundTransferStatusRepository _fundTransferRepository;

        public FundTransferService(IAccountHttpClientService accountHttpClientService, IFundTransferStatusRepository fundTransferRepository) {
            _accountHttpClientService = accountHttpClientService;
            _fundTransferRepository = fundTransferRepository;
            _factory = new ConnectionFactory();
            _factory.UserName = "acesso";
            _factory.Password = "acesso";
            _factory.HostName = "rabbitmq";
            _factory.DispatchConsumersAsync = true;
        }

        public FundTransferResultVM SendTransferRequestToQueue(FundTransferRequestVM fundTransferVM) {

            //Generate Transaction Key
            var transactionId = GenerateTransactionId();

            // Update Status In Queue
            SetFundTransferStatus(transactionId, "In Queue");

            var fundTransferQueueVM = new FundTransferQueueVM(transactionId, fundTransferVM);            

            string message = JsonSerializer.Serialize(fundTransferQueueVM);
            var body = Encoding.UTF8.GetBytes(message);

            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                channel.BasicPublish(exchange: "fund.exchange",
                                        routingKey: "q.fundtransfer.request",
                                        basicProperties: null,
                                        body: body);
            }
            Console.WriteLine(" [x] Sent {0}", message);

            return new FundTransferResultVM() { transactionId = transactionId };
        }

        public void ProcessFundTransferQueue() {

            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                channel.BasicQos(0, 1, false);
                
                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += FundTransferQueueReceived;
                channel.BasicConsume(queue: "q.fundtransfer.request",
                                     autoAck: false,
                                     consumer: consumer);

                Console.ReadLine();
            }

        }

        private async Task FundTransferQueueReceived(object sender, BasicDeliverEventArgs ea) {

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var fundTransferQueueVM = JsonSerializer.Deserialize<FundTransferQueueVM>(message);            

            try {
                
                // Update Status Processing
                SetFundTransferStatus(fundTransferQueueVM.transactionId, "Processing");

                // Get Accounts
                var accountOrigin = await _accountHttpClientService.Get(fundTransferQueueVM.accountOrigin);
                var accountDestination = await _accountHttpClientService.Get(fundTransferQueueVM.accountDestination);

                // Credit
                var creditRequest = new BalanceAdjustmentRequestVM() { accountNumber = accountDestination.accountNumber, value = fundTransferQueueVM.value, type = "Credit" };
                var creditResponse = await _accountHttpClientService.TransactionRequest(creditRequest);
                if (!creditResponse.IsSuccessStatusCode) throw new Exception("Error posting 'Credit'");

                // Debit
                var debitRequest = new BalanceAdjustmentRequestVM() { accountNumber = accountOrigin.accountNumber, value = fundTransferQueueVM.value, type = "Debit" };
                var debitResponse = await _accountHttpClientService.TransactionRequest(debitRequest);
                if (!debitResponse.IsSuccessStatusCode) throw new Exception("Error posting 'Debit'");

                // Update Status Confirmed
                SetFundTransferStatus(fundTransferQueueVM.transactionId, "Confirmed");

                //Ack Queue
                ((AsyncDefaultBasicConsumer)sender).Model.BasicAck(ea.DeliveryTag, false);

            } catch (Exception ex) {
                //Nack Queue
                ((AsyncDefaultBasicConsumer)sender).Model.BasicNack(ea.DeliveryTag, false, false);
                // Update Status Error
                SetFundTransferStatus(fundTransferQueueVM.transactionId, "Error", ex.Message);
            }

        }

        private void SetFundTransferStatus(string transactionId, string transactionStatus, string message = "") {

            var fundTransferStatus = _fundTransferRepository.GetByTransactionId(transactionId);

            if(fundTransferStatus == null) {
                fundTransferStatus = new FundTransferStatus() {
                    TransactionId = transactionId,
                    Status = transactionStatus,
                    Message = message
                };
                _fundTransferRepository.Add(fundTransferStatus);
            } else {
                fundTransferStatus.Status = transactionStatus;
                fundTransferStatus.Message = message;
                _fundTransferRepository.Replace(fundTransferStatus);
            }            
        }

        private string GenerateTransactionId() {
            return Guid.NewGuid().ToString();
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
        }

    }
}
