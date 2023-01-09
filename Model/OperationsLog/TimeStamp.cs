namespace B2B_Utils.Model
{
    public partial class TimeStamp
    {
        private readonly string timeStamp;
        public TimeStamp(string time)
        {
            this.timeStamp = time;
        }

        public string Date
        {
            get
            {
                return timeStamp.Substring(0, 10);
            }
        }

        public string Time
        {
            get
            {
                return timeStamp.Substring(11, 12);
            }
        }
    }
}
