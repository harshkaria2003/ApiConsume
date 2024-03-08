

namespace ApiConsume.Areas.DashBoard.Models
{
    public class Count
    {
        public int airlinesCount {  get; set; }

        public int airportsCount { get; set; }

        public int customersCount { get; set; }

        public int flightsCount { get; set; }


    }
	

	public class DashBoardModel
	{
		public bool status { get; set; }
		public string message { get; set; }
		public List<Count> data { get; set; }
	}
}
