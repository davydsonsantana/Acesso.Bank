using Acesso.Application.Bank.Interface;
using Acesso.Application.Bank.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<FundTransferResultVM> Post([FromBody]FundTransferRequestVM fundTransferModel) {

            try {
                _logger.LogInformation("Acesso.Services.FundTransfer POST: {time} | accountOrigin: {accOrigin} - accountDestination: {accDest} - value: {value}", DateTimeOffset.Now, fundTransferModel.accountOrigin, fundTransferModel.accountDestination, fundTransferModel.value);
                var transferResult = _fundTransferService.SendTransferRequestToQueue(fundTransferModel);
                return Accepted(transferResult);

            } catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
    }
}
