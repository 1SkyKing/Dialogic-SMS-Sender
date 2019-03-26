using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dlgc.Data.Models
{
    /// <summary>
    /// Veri tabanında Null olabilecek tüm alanları nullable olarak işaretlemek gerek.!!!
    /// </summary>
    [Table("Messages")]
    public class Messages
    {
        #region ID
        [Key]
        public long ID { get; set; }
        #endregion

        #region TypeID
        public int TypeID { get; set; }
        #endregion

        #region StatusID
        public int StatusID { get; set; }
        #endregion

        #region StatusDetailsID
        public int StatusDetailsID { get; set; }
        #endregion

        #region ChannelID
        public int ChannelID { get; set; }
        #endregion

        #region BillingID
        private string _BillingID = "";
        public string BillingID
        {
            get { return _BillingID; }
            set { _BillingID = value; }
        }
        #endregion

        #region ScheduledTime
        private DateTime? _ScheduledTime = new DateTime();
        public DateTime? ScheduledTime
        {
            get { return _ScheduledTime; }
            set { _ScheduledTime = value; }
        }
        #endregion

        #region EndTime
        private DateTime? _EndTime = new DateTime();
        public DateTime? EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }
        #endregion

        #region SentTime
        private DateTime? _SentTime = new DateTime();
        public DateTime? SentTime
        {
            get { return _SentTime; }
            set { _SentTime = value; }
        }
        #endregion

        #region ReceivedTime
        private DateTime? _ReceivedTime = new DateTime();
        public DateTime? ReceivedTime
        {
            get { return _ReceivedTime; }
            set { _ReceivedTime = value; }
        }
        #endregion

        #region LastUpdate
        private DateTime? _LastUpdate = new DateTime();
        public DateTime? LastUpdate
        {
            get { return _LastUpdate; }
            set { _LastUpdate = value; }
        }
        #endregion

        #region FromAddress
        private string _FromAddress = "";
        public string FromAddress
        {
            get { return _FromAddress; }
            set { _FromAddress = value; }
        }
        #endregion

        #region Priority
        public int Priority { get; set; }
        #endregion

        #region ReadReceipt
        public bool ReadReceipt { get; set; }
        #endregion

        #region Subject
        private string _Subject = "";
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }
        #endregion

        #region BodyFormatID
        public int BodyFormatID { get; set; }
        #endregion

        #region CharsetID
        /// <summary>
        ///  1:Latin, 2: TR, 3:UCS2
        /// </summary>
        public int CharsetID { get; set; }
        #endregion

        #region Modifier
        public int Modifier { get; set; }
        #endregion

        #region GWID
        public decimal GWID { get; set; }
        #endregion

        #region CustomField
        private string _CustomField = "";
        public string CustomField
        {
            get { return _CustomField; }
            set { _CustomField = value; }
        }
        #endregion

        #region sysLock
        public bool sysLock { get; set; }
        #endregion

        #region sysHash
        private string _sysHash = "";
        public string sysHash
        {
            get { return _sysHash; }
            set { _sysHash = value; }
        }
        #endregion

        #region sysCreator
        public int sysCreator { get; set; }
        #endregion

        #region sysArchive
        public bool sysArchive { get; set; }
        #endregion

        #region MessageReference
        private string _MessageReference = "";
        public string MessageReference
        {
            get { return _MessageReference; }
            set { _MessageReference = value; }
        }
        #endregion

        #region ToAddress
        private string _ToAddress = "";
        public string ToAddress
        {
            get { return _ToAddress; }
            set { _ToAddress = value; }
        }
        #endregion

        #region Header
        private string _Header = "";
        public string Header
        {
            get { return _Header; }
            set { _Header = value; }
        }
        #endregion

        #region Body
        private string _Body = "";
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }
        #endregion

        #region Trace
        private string _Trace = "";
        public string Trace
        {
            get { return _Trace; }
            set { _Trace = value; }
        }
        #endregion


    }
}
