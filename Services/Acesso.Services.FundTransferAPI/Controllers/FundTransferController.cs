using Acesso.Application.Bank.Interface;
using Acesso.Application.Bank.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Dynamic;
using System.Text;
using System.Text.Json;

namespace Acesso.Services.FundTransferAPI.Controllers {
    [ApiController]
    [Route("api/fund-transfer")]
    public class FundTransferController : ControllerBase {

        private readonly ILogger<FundTransferController> _logger;
        private readonly IFundTransferService _fundTransferService;

        public FundTransferController(ILogger<FundTransferController> logger, IFundTransferService fundTransferService) {
            _logger = logger;
            _fundTransferService = fundTransferService;
        }

        [HttpPost]
        public ActionResult<FundTransferResultVM> Post([FromBody] FundTransferRequestVM fundTransferModel) {

            try {
                _logger.LogInformation("Acesso.Services.FundTransfer POST: {time} | accountOrigin: {accOrigin} - accountDestination: {accDest} - value: {value}", DateTimeOffset.Now, fundTransferModel.accountOrigin, fundTransferModel.accountDestination, fundTransferModel.value);
                var transferResult = _fundTransferService.SendTransferRequestToQueue(fundTransferModel);
                return Accepted(transferResult);

            } catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{transactionId}")]
        public ActionResult<object> Get(string transactionId) {

            try {
                var fundTransferStatus = _fundTransferService.GetFundTransferStatus(transactionId);

                dynamic result = new ExpandoObject();
                
                if (fundTransferStatus == null) { // Transaction not found
                    result.Status = "Error";
                    result.Message = "TransactionId not found!";
                }else if (fundTransferStatus.Status == "Error") { // Transaction error
                    result.Status = "Error";
                    result.Message = fundTransferStatus.Message;
                } else {
                    result.Status = fundTransferStatus.Status;
                }

                return Ok(result);

            } catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
