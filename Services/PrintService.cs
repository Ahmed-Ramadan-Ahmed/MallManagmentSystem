using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using MallManagmentSystem.Data;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Services
{
    public class PrintService : IPrintService
    {
        private readonly MallDBContext _context;
        private readonly IStatisticsService _statisticsService;

        public PrintService(MallDBContext context, IStatisticsService statisticsService)
        {
            _context = context;
            _statisticsService = statisticsService;
        }

        public async Task<byte[]> PrintRentInvoiceAsync(int invoiceId)
        {
            try
            {
                var invoice = await _context.RentInvoices
                    .Include(i => i.Store)
                    .Include(i => i.Renter)
                    .FirstOrDefaultAsync(i => i.Id == invoiceId);

                if (invoice == null)
                    throw new Exception($"Invoice {invoiceId} not found");

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Rent Invoice");

                    // Add invoice details
                    var table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    AddTableRow(table, "Invoice Number", invoice.Id.ToString());
                    AddTableRow(table, "Store", invoice.Store.Name);
                    AddTableRow(table, "Renter", invoice.Renter.Name);
                    AddTableRow(table, "Invoice Date", invoice.InvoiceDate.ToShortDateString());
                    AddTableRow(table, "Due Date", invoice.DueDate.ToShortDateString());
                    AddTableRow(table, "Total Amount", invoice.Amount.ToString("C"));
                    AddTableRow(table, "Remaining Amount", invoice.RemainAmount.ToString("C"));
                    AddTableRow(table, "Status", invoice.IsPending ? "Pending" : "Paid");

                    document.Add(table);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to print invoice {invoiceId}", ex);
            }
        }

        public async Task<byte[]> PrintRentInvoiceWithPaymentsAsync(int invoiceId)
        {
            try
            {
                var invoice = await _context.RentInvoices
                    .Include(i => i.Store)
                    .Include(i => i.Renter)
                    .FirstOrDefaultAsync(i => i.Id == invoiceId);

                if (invoice == null)
                    throw new Exception($"Invoice {invoiceId} not found");

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Rent Invoice with Payment History");

                    // Add invoice details
                    var invoiceTable = new Table(2);
                    invoiceTable.SetWidth(UnitValue.CreatePercentValue(100));

                    AddTableRow(invoiceTable, "Invoice Number", invoice.Id.ToString());
                    AddTableRow(invoiceTable, "Store", invoice.Store.Name);
                    AddTableRow(invoiceTable, "Renter", invoice.Renter.Name);
                    AddTableRow(invoiceTable, "Invoice Date", invoice.InvoiceDate.ToShortDateString());
                    AddTableRow(invoiceTable, "Due Date", invoice.DueDate.ToShortDateString());
                    AddTableRow(invoiceTable, "Total Amount", invoice.Amount.ToString("C"));
                    AddTableRow(invoiceTable, "Remaining Amount", invoice.RemainAmount.ToString("C"));
                    AddTableRow(invoiceTable, "Status", invoice.IsPending ? "Pending" : "Paid");

                    document.Add(invoiceTable);

                    // Add payment history
                    document.Add(new Paragraph("Payment History").SetFontSize(15));

                    var paymentTable = new Table(5);
                    paymentTable.SetWidth(UnitValue.CreatePercentValue(100));

                    AddTableHeader(paymentTable, "Date", "Amount", "Cash", "Services", "Transaction ID");

                    AddTableRow(paymentTable,
                            invoice.InvoiceDate.ToShortDateString(),
                            invoice.Amount.ToString("C"),
                            invoice.RemainAmount.ToString(),
                            invoice.DebitAmount.ToString(),
                            invoice.Id.ToString());

                    document.Add(paymentTable);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to print invoice with payments {invoiceId}", ex);
            }
        }

        public async Task<byte[]> PrintMultipleInvoicesAsync(List<int> invoiceIds)
        {
            try
            {
                var invoices = await _context.RentInvoices
                    .Include(i => i.Store)
                    .Include(i => i.Renter)
                    .Where(i => invoiceIds.Contains(i.Id))
                    .ToListAsync();

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Multiple Invoices Report");

                    foreach (var invoice in invoices)
                    {
                        // Replace all occurrences of `.SetBold()` with `.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))`
                        // This change ensures that the text is set to bold using the appropriate method provided by the iText library.

                        document.Add(new Paragraph($"Invoice #{invoice.Id}")
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                            .SetFontSize(14));

                        var table = new Table(2);
                        table.SetWidth(UnitValue.CreatePercentValue(100));

                        AddTableRow(table, "Store", invoice.Store.Name);
                        AddTableRow(table, "Renter", invoice.Renter.Name);
                        AddTableRow(table, "Invoice Date", invoice.InvoiceDate.ToShortDateString());
                        AddTableRow(table, "Due Date", invoice.DueDate.ToShortDateString());
                        AddTableRow(table, "Total Amount", invoice.Amount.ToString("C"));
                        AddTableRow(table, "Remaining Amount", invoice.RemainAmount.ToString("C"));
                        AddTableRow(table, "Status", invoice.IsPending ? "Pending" : "Paid" );

                        document.Add(table);
                        document.Add(new Paragraph("\n"));
                    }

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to print multiple invoices", ex);
            }
        }

        public async Task<byte[]> PrintStoreContractAsync(int contractId)
        {
            try
            {
                var contract = await _context.StoreRentContracts
                    .Include(c => c.Store)
                    .Include(c => c.Renter)
                    .FirstOrDefaultAsync(c => c.Id == contractId);

                if (contract == null)
                    throw new Exception($"Contract {contractId} not found");

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Store Rent Contract");

                    // Add contract details
                    var table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    AddTableRow(table, "Contract Number", contract.Id.ToString());
                    AddTableRow(table, "Store", contract.Store.Name);
                    AddTableRow(table, "Renter", contract.Renter.Name);
                    AddTableRow(table, "Start Date", contract.StartDate.ToShortDateString());
                    AddTableRow(table, "End Date", contract.EndDate.ToShortDateString());
                    AddTableRow(table, "Monthly Rent", contract.MonthlyRent.ToString("C"));
                    AddTableRow(table, "Status", contract.Status);

                    document.Add(table);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to print store contract {contractId}", ex);
            }
        }

        public async Task<byte[]> PrintEmployeeContractAsync(int contractId)
        {
            try
            {
                var contract = await _context.EmploymentContracts
                    .Include(c => c.Employee)
                    .FirstOrDefaultAsync(c => c.Id == contractId);

                if (contract == null)
                    throw new Exception($"Contract {contractId} not found");

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Employee Contract");

                    // Add contract details
                    var table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    AddTableRow(table, "Contract Number", contract.Id.ToString());
                    AddTableRow(table, "Employee", contract.Employee.Name);
                    AddTableRow(table, "Position", contract.JobDescription);
                    AddTableRow(table, "Start Date", contract.StartDate.ToShortDateString());
                    AddTableRow(table, "End Date", contract.EndDate.ToShortDateString());
                    AddTableRow(table, "Salary", contract.Salary.ToString("C"));
                    AddTableRow(table, "Status", contract.Status);

                    document.Add(table);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to print employee contract {contractId}", ex);
            }
        }

        public async Task<byte[]> PrintMultipleContractsAsync(List<int> contractIds)
        {
            try
            {
                var storeContracts = await _context.StoreRentContracts
                    .Include(c => c.Store)
                    .Include(c => c.Renter)
                    .Where(c => contractIds.Contains(c.Id))
                    .ToListAsync();

                var employeeContracts = await _context.EmploymentContracts
                    .Include(c => c.Employee)
                    .Where(c => contractIds.Contains(c.Id))
                    .ToListAsync();

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Multiple Contracts Report");

                    if (storeContracts.Any())
                    {
                        document.Add(new Paragraph("Store Rent Contracts")
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                            .SetFontSize(14));
                        foreach (var contract in storeContracts)
                        {
                            AddContractDetails(document, contract);
                        }
                    }

                    if (employeeContracts.Any())
                    {
                        document.Add(new Paragraph("Employee Contracts")
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                            .SetFontSize(14));
                        foreach (var contract in employeeContracts)
                        {
                            AddContractDetails(document, contract);
                        }
                    }

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to print multiple contracts", ex);
            }
        }

        public async Task<byte[]> PrintFinancialStatisticsAsync(int mallId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var stats = await _statisticsService.GetFinancialStatisticsAsync(mallId, startDate, endDate);

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Financial Statistics Report");

                    // Add statistics
                    var table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    AddTableRow(table, "Total Revenue", stats.TotalRevenue.ToString("C"));
                    AddTableRow(table, "Total Expenses", stats.TotalExpenses.ToString("C"));
                    AddTableRow(table, "Net Income", stats.NetProfit.ToString("C"));
                    AddTableRow(table, "Total Rent Collected", stats.TotalRentCollected.ToString("C"));
                    AddTableRow(table, "Total Rent Due", stats.TotalRentDue.ToString("C"));
                    AddTableRow(table, "Total Rent Overdue", stats.TotalRentOverdue.ToString("C"));

                    document.Add(table);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to print financial statistics", ex);
            }
        }

        public async Task<byte[]> PrintStoreStatisticsAsync(int mallId)
        {
            try
            {
                var stats = await _statisticsService.GetStoreStatisticsAsync(mallId);

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Store Statistics Report");

                    // Add statistics
                    var table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    AddTableRow(table, "Total Stores", stats.TotalStores.ToString());
                    AddTableRow(table, "Occupied Stores", stats.OccupiedStores.ToString());
                    AddTableRow(table, "Vacant Stores", stats.VacantStores.ToString());
                    AddTableRow(table, "Occupancy Rate", $"{stats.OccupancyRate}%");

                    document.Add(table);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to print store statistics", ex);
            }
        }

        public async Task<byte[]> PrintEmployeeStatisticsAsync(int mallId)
        {
            try
            {
                var stats = await _statisticsService.GetEmployeeStatisticsAsync(mallId);

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Employee Statistics Report");

                    // Add statistics
                    var table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    AddTableRow(table, "Total Employees", stats.TotalEmployees.ToString());
                    AddTableRow(table, "Active Employees", stats.ActiveEmployees.ToString());
                    AddTableRow(table, "Average Salary", stats.AverageSalary.ToString("C"));
                    AddTableRow(table, "Total Salary Expense", stats.TotalSalaryPayments.ToString("C"));

                    document.Add(table);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to print employee statistics", ex);
            }
        }

        public async Task<byte[]> PrintRenterStatisticsAsync(int mallId)
        {
            try
            {
                var stats = await _statisticsService.GetRenterStatisticsAsync(mallId);

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Renter Statistics Report");

                    // Add statistics
                    var table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    AddTableRow(table, "Total Renters", stats.TotalRenters.ToString());
                    AddTableRow(table, "Active Renters", stats.ActiveRenters.ToString());
                    AddTableRow(table, "Total Rent Due", stats.TotalRentDue.ToString("C"));
                    AddTableRow(table, "Total Rent Overdue", stats.TotalRentOverdue.ToString("C"));

                    document.Add(table);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to print renter statistics", ex);
            }
        }

        public async Task<byte[]> PrintContractStatisticsAsync(int mallId)
        {
            try
            {
                var stats = await _statisticsService.GetContractStatisticsAsync(mallId);

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add header
                    AddHeader(document, "Contract Statistics Report");

                    // Add statistics
                    var table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    AddTableRow(table, "Total Contracts", stats.TotalContracts.ToString());
                    AddTableRow(table, "Active Contracts", stats.ActiveContracts.ToString());
                    AddTableRow(table, "Expiring Contracts", stats.ExpiringContracts.ToString());
                    AddTableRow(table, "Expired Contracts", stats.ExpiredContracts.ToString());

                    document.Add(table);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to print contract statistics", ex);
            }
        }

        public async Task<byte[]> GenerateRevenueChartAsync(int mallId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var stats = await _statisticsService.GetFinancialStatisticsAsync(mallId, startDate, endDate);

                // Generate chart using Chart.js
                var chartData = new
                {
                    type = "line",
                    data = new
                    {
                        labels = new[] { "Revenue", "Expenses", "Net Income" },
                        datasets = new[]
                        {
                            new
                            {
                                label = "Financial Overview",
                                data = new[] { stats.TotalRevenue, stats.TotalExpenses, stats.NetProfit },
                                backgroundColor = new[] { "#4CAF50", "#F44336", "#2196F3" }
                            }
                        }
                    }
                };

                // Convert chart to image and return as byte array
                return await ConvertChartToImageAsync(chartData);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate revenue chart", ex);
            }
        }

        public async Task<byte[]> GenerateOccupancyChartAsync(int mallId)
        {
            try
            {
                var stats = await _statisticsService.GetStoreStatisticsAsync(mallId);

                // Generate chart using Chart.js
                var chartData = new
                {
                    type = "pie",
                    data = new
                    {
                        labels = new[] { "Occupied", "Vacant" },
                        datasets = new[]
                        {
                            new
                            {
                                data = new[] { stats.OccupiedStores, stats.VacantStores },
                                backgroundColor = new[] { "#4CAF50", "#F44336" }
                            }
                        }
                    }
                };

                // Convert chart to image and return as byte array
                return await ConvertChartToImageAsync(chartData);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate occupancy chart", ex);
            }
        }

        public async Task<byte[]> GeneratePaymentStatusChartAsync(int mallId)
        {
            try
            {
                var stats = await _statisticsService.GetRenterStatisticsAsync(mallId);

                // Generate chart using Chart.js
                var chartData = new
                {
                    type = "bar",
                    data = new
                    {
                        labels = new[] { "Rent Due", "Rent Overdue" },
                        datasets = new[]
                        {
                            new
                            {
                                label = "Payment Status",
                                data = new[] { stats.TotalRentDue, stats.TotalRentOverdue },
                                backgroundColor = new[] { "#FFC107", "#F44336" }
                            }
                        }
                    }
                };

                // Convert chart to image and return as byte array
                return await ConvertChartToImageAsync(chartData);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate payment status chart", ex);
            }
        }

        public async Task<byte[]> GenerateEmployeePerformanceChartAsync(int mallId)
        {
            try
            {
                var stats = await _statisticsService.GetEmployeeStatisticsAsync(mallId);

                // Generate chart using Chart.js
                var chartData = new
                {
                    type = "doughnut",
                    data = new
                    {
                        labels = new[] { "Active", "Inactive" },
                        datasets = new[]
                        {
                            new
                            {
                                data = new[] { stats.ActiveEmployees, stats.TotalEmployees - stats.ActiveEmployees },
                                backgroundColor = new[] { "#4CAF50", "#9E9E9E" }
                            }
                        }
                    }
                };

                // Convert chart to image and return as byte array
                return await ConvertChartToImageAsync(chartData);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate employee performance chart", ex);
            }
        }

        // Replace all occurrences of `.SetBold()` with `.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))`
        // This change ensures that the text is set to bold using the appropriate method provided by the iText library.

        private void AddHeader(Document document, string title)
        {
            var header = new Paragraph(title)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)); // Updated to set bold font
            document.Add(header);
            document.Add(new Paragraph("\n"));
        }

        private void AddFooter(Document document)
        {
            var footer = new Paragraph($"Generated on {DateTime.Now:g}")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(10)
                .SetFontColor(ColorConstants.GRAY)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)); // Updated to set bold font
            document.Add(new Paragraph("\n"));
            document.Add(footer);
        }

        private void AddTableRow(Table table, params string[] headers)
        {
            foreach (var header in headers)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(header)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)))); // Updated to set bold font
            }
        }

        private void AddTableHeader(Table table, params string[] headers)
        {
            foreach (var header in headers)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(header)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)))); // Updated to set bold font
            }
        }

        private void AddContractDetails(Document document, dynamic contract)
        {
            var table = new Table(2);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            AddTableRow(table, "Contract Number", contract.Id.ToString());
            AddTableRow(table, "Start Date", contract.StartDate.ToShortDateString());
            AddTableRow(table, "End Date", contract.EndDate.ToShortDateString());
            AddTableRow(table, "Status", contract.Status);

            document.Add(table);
            document.Add(new Paragraph("\n"));
        }

        private async Task<byte[]> ConvertChartToImageAsync(dynamic chartData)
        {
            // TODO: Implement chart to image conversion
            // This would typically involve:
            // 1. Converting chart data to JSON
            // 2. Using a headless browser or chart rendering service
            // 3. Capturing the rendered chart as an image
            // 4. Converting the image to a byte array
            throw new NotImplementedException("Chart to image conversion not implemented");
        }
    }
} 