using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Manager
{
    public class ManagerCore
    {
        public static readonly ManagerCore Instance = new ManagerCore();

        private ManagerCore()
        {
            foreach (var p in this.GetType().GetProperties())
            {
                if (p.PropertyType == this.GetType())
                {
                    continue;
                }
                var val = p.GetValue(this);
                if (val == null)
                {
                    p.SetValue(this, Activator.CreateInstance(p.PropertyType));
                }
            }
        }

        public UserManager UserManager { get; private set; }
        public UserGroupManager UserGroupManager { get; private set; }

        public TaskManager TaskManager { get; private set; }

        public CommentManager CommentManager { get; private set; }

        public AttachmentManager AttachmentManager { get; private set; }

        public AuthorizeManager AuthorizeManager { get; private set; }

        public DataBookManager DataBookManager { get; private set; }

        public MessageManager MessageManager { get; private set; }

        public NotificationManager NotificationManager { get; private set; }

        public QueryManager QueryManager { get; private set; }
       
        public BillManager BillManager { get; private set; }
        public ContractManager ContractManager { get; private set; }
        public InvoiceManager InvoiceManager { get; private set; }
        public BankManager BankManager { get; private set; }
        public ContractFileManager ContractFileManager { get; private set; }
        public BillAccountManager BillAccountManager { get; private set; }
        public ArticleManager ArticleManager { get; private set; }
        public ContractArticleManager ContractArticleManager { get; private set; }
        public BillContractManager BillContractManager { get; private set; }

        #region  项目管理系统
        
        public CityManager CityManager { get; private set; }
        public Project_TypeManager Project_TypeManager { get; private set; }
        public Bill_ViewManager Bill_ViewManager { get; private set; }
        public ProjectManager ProjectManager { get; private set; }
        public ProjectUserManager ProjectUserManager { get; private set; }
        public Project_ProgressManager Project_ProgressManager { get; private set; }
        public ProjectRecordManager ProjectRecordManager { get; private set; }
        public WorkloadManager WorkloadManager { get; private set; }

        #endregion


        #region  权限管理系统

        public AD_GroupManager AD_groupManager { get; private set; }

        public Authorize_FastManager Authorize_FastManager { get; private set; }
        #endregion

        #region  银行对账系统

        public Bill_OneManager Bill_OneManager { get; private set; }

        public Bill_RecordManager Bill_RecordManager { get; private set; }
        public Bill_Records_ViewManager Bill_Records_ViewManager { get; private set; }

        #endregion

        #region 平板管理系统

        public iPadManager iPadManager { get; private set; }

        public iPad_InvoiceManager iPad_InvoiceManager { get; private set; }

        public iPad_RegisterManager iPad_registerManager { get; private set; }

        public Register_iPadManager Register_iPadManager { get; private set; }

        public iPad_ContractManager iPad_ContractManager { get; private set; }

        public iPad_AccountManager iPad_AccountManager { get; private set; }
        public iPad_ContactManager iPad_ContactManager { get; private set; }
        public iPad_DatumManager iPad_DatumManager { get; private set; }

        #endregion

        #region 任务系统



        #endregion

        #region  报销系统

        //public SerialNumberManager SerialNumberManager { get; private set; }

        public SheetManager SheetManager { get; private set; }
        public SheetViewManager SheetViewManager { get; private set; }

        public SubstancsManager SubstanceManager { get; private set; }

        public VerifyManager VerifyManager { get; private set; }
        public VerifyViewManager VerifyViewManager { get; private set; }
        public Report_TypeManager Report_TypeManager { get; private set; }
        public Report_ManagerManager Report_ManagerManager { get; private set; }
        /// <summary>
        /// 招待
        /// </summary>
        public ReceptionManager ReceptionManager { get; private set; }
        #endregion


        #region 系统配置

        public OASystemManager OASystemManager { get; private set; }
        public PowerManager PowerManager { get; private set; }
        public Power_itemManager Power_itemManager { get; private set; }
        public Client_MessageManager Client_MessageManager { get; private set; }
        #endregion
    }
}
