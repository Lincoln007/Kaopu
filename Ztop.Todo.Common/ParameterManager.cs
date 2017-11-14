namespace Ztop.Todo.Common
{
    public static class ParameterManager
    {
        public  const string ExcelContentType = "application/ms-excel";

        public const string TimeContentType = "yyyy-MM-dd HH-mm-ss";

        public const string ShentuKey = "shentuSheet";
        public const string ParameterKey = "Parameter";

        private static string _shentuFilePath { get; set; }
        /// <summary>
        /// 申屠对应的报销表格
        /// </summary>
        public static string ShentuFilePath { get { return string.IsNullOrEmpty(_shentuFilePath) ? _shentuFilePath = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["20170930SHENTU"]) : _shentuFilePath; } }
        private static string _shentuDaily { get; set; }
        /// <summary>
        /// 日常报销单文件
        /// </summary>
        public static string ShentuDaily { get { return string.IsNullOrEmpty(_shentuDaily) ? _shentuDaily = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["20170930DAILY"]) : _shentuDaily; } }
        private static string _shentuErrand { get; set; }
        /// <summary>
        /// 出差报销单文件
        /// </summary>
        public static string SHentuErrand { get { return string.IsNullOrEmpty(_shentuErrand) ? _shentuErrand = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["20170930ERRAND"]) : _shentuErrand; } }
        private static string _shentuTransfer { get; set; }
        /// <summary>
        /// 转账支出单文件
        /// </summary>
        public static string ShentuTransfer { get { return string.IsNullOrEmpty(_shentuTransfer) ? _shentuTransfer = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["20170930TRANSFER"]) : _shentuTransfer; } }
        private static string _shentuReception { get; set; }
        /// <summary>
        /// 日常招待单文件
        /// </summary>
        public static string ShentuReception { get { return string.IsNullOrEmpty(_shentuReception) ? _shentuReception = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["20170930RECEPTION"]) : _shentuReception; } }
    }
}
