﻿using Acesso.Application.Bank.ViewModel;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Acesso.Application.Bank.Interface;
using RabbitMQ.Client.Events;
using System.Net.Http;

namespace Acesso.Application.Bank.Service {
    public class FundTransferService : IFundTransferService {
        
        private readonly ConnectionFactory _factory;
        private readonly IAccountService _accountService;
                
        public FundTransferService(IAccountService accountService) {
            _accountService = accountService;
            _factory = new ConnectionFactory();
            _factory.UserName = "acesso";
            _factory.Password = "acesso";
            _factory.HostName = "rabbitmq";
            _factory.DispatchConsumersAsync = true;
        }

        public FundTransferResultVM SendTransferRequestToQueue(FundTransferRequestVM fundTransferVM) {

            var transactionId = GenerateTransactionId();
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

            try {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var fundTransferQueueVM = JsonSerializer.Deserialize<FundTransferQueueVM>(message);
                    Console.WriteLine(" [x] Received {0}", message);
                
                var accountOrigin = await _accountService.Get(fundTransferQueueVM.accountOrigin);
                var accountDestination = await _accountService.Get(fundTransferQueueVM.accountDestination);
                
                ((AsyncDefaultBasicConsumer)sender).Model.BasicAck(ea.DeliveryTag, false);
            } catch (Exception ex) {
                ((AsyncDefaultBasicConsumer)sender).Model.BasicNack(ea.DeliveryTag, false, false);
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