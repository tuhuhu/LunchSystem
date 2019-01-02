using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iEnterAsia.iseiQ.Models
{
    public class EstimateInfo
    {
        public long EST_SEQ_NO { get; set; }
        public string EST_CONDITION { get; set; }
    }

    public class ContractPayments
    {
        public ContractPayments()
        {
            CONTRACT_OPD = new List<ContractOrderPlacementDetailsEntity>();
            //CONTRACT_OPD.Add(new ContractOrderPlacementDetailsEntity()); // DEBUG

            CONTRACT_OP = new ContractOrderPlacementPlus();
        }
        public ContractOrderPlacementPlus CONTRACT_OP { get; set; }
        public IList<ContractOrderPlacementDetailsEntity> CONTRACT_OPD { get; set; }
    }
    public class ContractRegistModel
    {
        public int SUBCONTRACT_TYPE_ENABLE { get; set; }
        public EstimateInfo ESTIMATE_INFO { get; set; }
        public ProjectMemoEntity PROJECT_MEMO { get; set; }
        public ContractPlus CONTRACT { get; set; }
        public IList<ContractOrderAcceptanceItemsPlus> CONTRACT_OAI { get; set; }
        public ContractOrderAcceptancePlus CONTRACT_OA { get; set; }
        public IList<ContractOrderAcceptanceDetailsEntity> CONTRACT_OAD { get; set; }
        public ContractOrderPlacementPlus CONTRACT_OP { get; set; }
        public IList<ContractOrderPlacementDetailsEntity> CONTRACT_OPD { get; set; }
        public IList<ContractPayments> CONTRACT_PAYMENTS { get; set; }
        public IList<SelectListItem> LIST_FIRM_CONTRACT { get; set; }
        public IList<SelectListItem> LIST_FIRM_CONTRACT_CLASS { get; set; }
        public IList<SelectListItem> LIST_PRODUCT { get; set; }
        public IList<EstimateAttachFileEntity> FILE_LIST { get; set; }
        public IList<EstimateAttachFileEntity> FILE_LIST_ATTACHED { get; set; } 

        public ContractRegistModel()
        {
            CONTRACT = new ContractPlus();
            CONTRACT_OA = new ContractOrderAcceptancePlus();
            CONTRACT_OAI = new List<ContractOrderAcceptanceItemsPlus>();
            CONTRACT_OAI.Add(new ContractOrderAcceptanceItemsPlus());
            CONTRACT_OAI[0].QTY = 1;
            CONTRACT_OAI[0].UNIT_TYPE = "式";

            CONTRACT_OAD = new List<ContractOrderAcceptanceDetailsEntity>();
            //CONTRACT_OAD.Add(new ContractOrderAcceptanceDetailsEntity()); // DEBUG


            CONTRACT_OP = new ContractOrderPlacementPlus();
            
            CONTRACT_OPD = new List<ContractOrderPlacementDetailsEntity>();
            //CONTRACT_OPD.Add(new ContractOrderPlacementDetailsEntity());

            ESTIMATE_INFO = new EstimateInfo();
            FILE_LIST = new List<EstimateAttachFileEntity>();
            FILE_LIST_ATTACHED = new List<EstimateAttachFileEntity>();

            //Delegation
            CONTRACT_PAYMENTS = new List<ContractPayments>();
            CONTRACT_PAYMENTS.Add(new ContractPayments());
        }
    }
}
