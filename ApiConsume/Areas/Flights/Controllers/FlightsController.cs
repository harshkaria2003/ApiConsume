using ApiConsume.Areas.Airports.Models;
using ApiConsume.Areas.Flights.Models;
using ApiConsume.BAL;
using ApiConsume.Models;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace ApiConsume.Areas.Flights.Controllers
{
    [CheckAccess]
    [Area("Flights")]
	[Route("Flights/{Controller}/{Action}")]
	public class FlightsController : Controller
	{

		string baseurl = "http://localhost:5231/api/";
		public async Task<IActionResult> Index()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseurl);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage responseMessage = await client.GetAsync("Flights/Get");
				if (responseMessage.IsSuccessStatusCode)
				{
					string data = responseMessage.Content.ReadAsStringAsync().Result;
					FlightListResponse jsonObject = JsonConvert.DeserializeObject<FlightListResponse>(data);
					var dataOfObject = jsonObject;
					return View("Index", dataOfObject);
				}
				else
				{
					Console.WriteLine("Error in api");
				}
				return View("Index");
			}
		}

        public DataTable FlightsList()
        {
            DataTable dataTable = new DataTable();
            using (var client = new HttpClient()) // Corrected variable name from _client to client
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{client.BaseAddress}Flights/Get").Result; // Corrected variable name from _client to client

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(data);
                    var dataOfObject = jsonObject.data;
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    dataTable = JsonConvert.DeserializeObject<DataTable>(extractdDatajson);
                }

                return dataTable;
            }
        }


        public IActionResult Export_Flights_List_To_Excel()
        {
            DataTable dataTable = FlightsList();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        IXLWorksheet ws = wb.Worksheets.Add("Flights  Report");

                        // Adding the DataTable data to the worksheet starting from cell A1
                        var tableRange = ws.Cell(1, 1).InsertTable(dataTable, true).AsRange();

                        // Adjust column widths to fit contents
                        ws.Columns().AdjustToContents();

                        // Apply styling to header row
                        var headerRow = tableRange.FirstRow();
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                        headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        // Set the row height and cell alignment for data rows
                        foreach (var row in tableRange.RowsUsed().Skip(1))
                        {
                            ws.Row(row.RowNumber()).Height = 20; // Set the height for the row
                            foreach (var cell in row.CellsUsed())
                            {
                                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            }
                        }

                        wb.SaveAs(memoryStream);

                        // Return the generated Excel file
                        string fileName = "Flight_Lists.xlsx";
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            else
            {
                // Handle the case where there's no data to export
                // For example, return a message indicating no data
                string message = "No data available to export.";
                byte[] data = Encoding.UTF8.GetBytes(message);
                return File(data, "text/plain", "NoData.txt");
            }
        }




        public IActionResult Export_Flights_List_To_pdf()
        {
            DataTable dataTable = FlightsList();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Custom page size
                    iTextSharp.text.Rectangle customPageSize = new iTextSharp.text.Rectangle(2300, 1200);
                    using (Document document = new Document(customPageSize))
                    {
                        PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStream);
                        document.Open();

                        // Define fonts
                        BaseFont boldBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.WINANSI, BaseFont.EMBEDDED);

                        Font boldFont = new Font(boldBaseFont, 12);

                        // Title
                        Paragraph title = new Paragraph("Flights  List", new Font(boldBaseFont, 35));
                        title.Alignment = Element.ALIGN_CENTER;
                        document.Add(title);
                        document.Add(new Chunk("\n"));

                        // Table setup
                        PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count)
                        {
                            WidthPercentage = 100,
                            DefaultCell = { Padding = 10 }
                        };

                        // Headers
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            Font headerFont = boldFont;
                            PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, headerFont))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 10
                            };
                            pdfTable.AddCell(headerCell);
                        }

                        // Data rows
                        foreach (DataRow row in dataTable.Rows)
                        {
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                var item = row[column];
                                Font itemFont = boldFont;

                                PdfPCell dataCell = new PdfPCell(new Phrase(item?.ToString(), itemFont))
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 10
                                };
                                pdfTable.AddCell(dataCell);
                            }
                        }

                        document.Add(pdfTable);
                        document.Close();
                    }

                    // Return the generated PDF file
                    string fileName = "Flight.pdf";
                    return File(memoryStream.ToArray(), "application/pdf", fileName);
                }
            }
            else
            {
                // Handle the case where there's no data to export
                // For example, return a message indicating no data
                string message = "No data available to export.";
                byte[] data = Encoding.UTF8.GetBytes(message);
                return File(data, "text/plain", "NoData.txt");
            }
        }

        public async Task<IActionResult> Edit(int id)
		{

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseurl);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage responseMessage = await client.GetAsync("Flights/Get/" + id);
				if (responseMessage.IsSuccessStatusCode)
				{
					string data = responseMessage.Content.ReadAsStringAsync().Result;
					FlightsResponse jsonObject = JsonConvert.DeserializeObject<FlightsResponse>(data);
					var dataOfObject = jsonObject.data;
					return View("InsertFlights", dataOfObject);



				}
				else
				{
					Console.WriteLine("Error in api");
				}


			}

			return View("InsertFlights");
		}

		public async Task<IActionResult> Insert(FlightsModel FlightsResponse)
		{
			using (var client = new HttpClient())
			{
				Console.WriteLine(FlightsResponse);
				client.BaseAddress = new Uri(baseurl);
				string data = JsonConvert.SerializeObject(FlightsResponse);
				StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
				if (FlightsResponse?.flight_id == null)
				{
					HttpResponseMessage responseMessage = await client.PostAsync("Flights/Insert", content);
					if (responseMessage.IsSuccessStatusCode)
					{
						return RedirectToAction("Index");
					}
					else
					{
						Console.WriteLine("Error in api");
					}
				}
				else
				{
					HttpResponseMessage responseMessage = await client.PutAsync("Flights/Update", content);
					if (responseMessage.IsSuccessStatusCode)
					{
						return RedirectToAction("Index");
					}
					else
					{
						Console.WriteLine("Error in api");
					}
				}
				return View("InsertFlights");
			}
		}

		public async Task<IActionResult> DeleteFlight(int id)
		{

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseurl);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage responseMessage = await client.DeleteAsync("Flights/Delete?flights_id=" + id);
				if (responseMessage.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");

				}
				else
				{
					return View("Index");
				}
			}
		}
	}
}
