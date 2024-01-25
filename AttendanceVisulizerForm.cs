using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using  Excel = Microsoft.Office.Interop.Excel;
namespace AttendanceVisualizer
{
    public partial class AttendanceVisulizerForm : Form
    {
        private readonly Dictionary<Employee, AttendanceCount> _attendanceRecords = new Dictionary<Employee, AttendanceCount>();

        public AttendanceVisulizerForm()
        {
            InitializeComponent();
        }

        public void ReadSheet_Button_Click(object sender, EventArgs e)
        {
            Excel.Workbook workbook = Globals.ThisAddIn.Application.ActiveWorkbook;
            Excel.Worksheet worksheet = workbook.Worksheets.Item[1];
            Excel.Range usedRange = worksheet.UsedRange;
            int numberOfRows = usedRange.Rows.Count;
            int row = 2;

            SheetContents_ListBox.Items.Clear();
            SheetContents_ListBox.Items.Add($"Number of Rows = {numberOfRows}");

            while (row <= numberOfRows)
            {
                ReadRow(worksheet, row);
                row += 1;
            }
        }

        public void ReadRow(Excel.Worksheet worksheet, int row)
        {
            string employeeData = worksheet.Cells[row, 1].Value.ToString();
            Employee employee = new Employee(employeeData);
            AttendanceCount attendance = new AttendanceCount();
            attendance.AddDate(worksheet.Cells[row, 2].Value.ToString());
            attendance.AddTimeIn(ParseExcelDate(worksheet.Cells[row, 3].Value));
            attendance.AddTimeOut(ParseExcelDate(worksheet.Cells[row, 4].Value));

            if (_attendanceRecords.Keys.Any(emp => emp.getID() == employee.getID()))
            {
                
                Employee existingEmployee = _attendanceRecords.Keys
                    .First(emp => emp.getID() == employee.getID());


                _attendanceRecords[existingEmployee] = attendance;
            }
            else
            {
                _attendanceRecords.Add(employee, attendance);
                SheetContents_ListBox.Items.Add(employee.getID());
            }

        }


        public int ParseExcelDate(object excelDate)
        {
            if (excelDate is double)
            {
                DateTime timeSpan = DateTime.FromOADate((double)excelDate);
                return timeSpan.Hour * 10000 + timeSpan.Minute * 100 + timeSpan.Second;
            }
            else
            {
                return 0;
            }
        }

        public void DrawButton_Click(object sender, EventArgs e)
        {
            Graphics g = TimeLineDrawingPanel.CreateGraphics();
            Brush txtBrush = new SolidBrush(Color.Black);
            StringFormat sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            int y = 25;
            int x = 100;

            for (int i = 0; i < 24; i++)
            {
                string period = i < 12 ? "AM" : "PM";
                int hour = i % 12;
                hour = hour == 0 ? 12 : hour;

                Point pDate = new Point(x, y);
                g.DrawString($"{hour}{period}", DrawButton.Font, txtBrush, pDate, sf);
                x += 40;
            }

            int county = 50;

            for (int i = 1; i <= 90; i++)
            {
                Point pDate = new Point(30, county);
                g.DrawString($"Day{i}", DrawButton.Font, txtBrush, pDate, sf);
                county += 25;
            }
        }

        public void AttendanceVisulizerForm_Load(object sender, EventArgs e)
        {
            SheetContents_ListBox.SelectedIndexChanged += SheetContents_ListBox_SelectedIndexChanged;
        }

        public void TimeLineDrawingPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        public void SheetContents_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEmployeeId = (string)SheetContents_ListBox.SelectedItem;

            Employee selectedEmployee = _attendanceRecords.Keys
                .FirstOrDefault(employee => employee.getID() == selectedEmployeeId);

            if (selectedEmployee == null) return;

            List<int> timeDifferences = _attendanceRecords[selectedEmployee].CalculateTimeDifferences();

            TimeLineDrawingPanel.Controls.Clear();  

            int y = 10;

            foreach (var timeDifference in timeDifferences)
            {
               
                int hours = timeDifference / 10000;
                int minutes = (timeDifference / 100) % 100;
                int totalMinutes = (hours * 60) + minutes;

                Panel linePanel = new Panel
                {
                    Size = new Size(totalMinutes, 10), 
                    Location = new Point(10, y),  
                    BackColor = Color.Green  
                };

                TimeLineDrawingPanel.Controls.Add(linePanel);  

                y += 100;  
            }
        }

    }
}

