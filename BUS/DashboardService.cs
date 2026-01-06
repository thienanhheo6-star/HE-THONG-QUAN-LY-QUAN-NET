using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;

namespace QLQuanNet
{
    public class DashboardStats
    {
        public decimal TodayRevenue { get; set; }
        public int ActiveMachines { get; set; }
        public int VIPMembers { get; set; }
        public decimal TotalBalance { get; set; }
        public int Warnings { get; set; }
    }

    public class DashboardService
    {
        public DashboardStats GetTodayStats()
        {
            using (var db = new Model1())
            {
                var today = DateTime.Today;
                // Lấy các phiên sử dụng liên quan đến ngày hôm nay.
                // Bao gồm phiên bắt đầu hôm nay hoặc kết thúc hôm nay (để tính đúng doanh thu phát sinh hôm nay).
                var todayUsage = db.SuDungMays
                    .Where(s => System.Data.Entity.DbFunctions.TruncateTime(s.ThoiGianBatDau) == today
                                || (s.ThoiGianKetThuc != null && System.Data.Entity.DbFunctions.TruncateTime(s.ThoiGianKetThuc) == today))
                    .ToList();

                // Lấy dịch vụ chỉ cho các phiên sử dụng của hôm nay
                var usageIds = todayUsage.Select(u => u.MaSD).ToList();
                var todayServices = db.SuDungDichVus
                    .Where(d => usageIds.Contains(d.MaSD))
                    .ToList();

                // Tổng doanh thu hôm nay: chỉ tính tiền nạp (NapTien) và tiền dịch vụ (SuDungDichVu)
                // Không bao gồm tiền giờ chơi từ SuDungMays.TongTien.
                var napTienToday = db.NapTiens
                    .Where(n => System.Data.Entity.DbFunctions.TruncateTime(n.ThoiGian) == today)
                    .Select(n => (decimal?)n.SoTien)
                    .ToList();
                decimal napSum = napTienToday.Sum() ?? 0m;

                var usageIdsToday = todayUsage.Select(u => u.MaSD).ToList();
                decimal svcSum = db.SuDungDichVus.Where(d => usageIdsToday.Contains(d.MaSD)).Select(d => (decimal?)d.ThanhTien).ToList().Sum() ?? 0m;

                decimal todayRevenue = napSum + svcSum;
                int activeCount = db.SuDungMays.Count(s => s.ThoiGianKetThuc == null);
                decimal totalBalance = db.TaiKhoanNguoiChois.Select(a => (decimal?)a.SoDu).Sum() ?? 0m;

                return new DashboardStats
                {
                    TodayRevenue = todayRevenue,
                    ActiveMachines = activeCount,
                    VIPMembers = 0,
                    TotalBalance = totalBalance,
                    Warnings = (activeCount > 10 ? 1 : 0)
                };
            }
        }

        public class RecentTransaction
        {
            public string EventType { get; set; }
            public int? Id { get; set; }
            public int? MaTK { get; set; }
            public DateTime Thoigian { get; set; }
            public decimal? TienNap { get; set; }
            public decimal? Services { get; set; }
            public string Account { get; set; }
        }

        public List<RecentTransaction> GetRecentTransactions(int count = 10)
        {
            using (var db = new Model1  ())
            {
                // Lấy N bản ghi nạp tiền gần nhất và N phiên sử dụng gần nhất
                var recentTopups = db.NapTiens
                    .OrderByDescending(n => n.ThoiGian)
                    .Take(count)
                    .ToList();

                var recentUses = db.SuDungMays
                    .OrderByDescending(s => s.ThoiGianBatDau)
                    .Take(count)
                    .ToList();

                // Dịch vụ theo các MaSD của phiên sử dụng đã lấy
                var usageIds = recentUses.Select(r => r.MaSD).ToList();
                var services = db.SuDungDichVus
                    .Where(d => usageIds.Contains(d.MaSD))
                    .Join(db.DICHVUs, sdv => sdv.MaDV, dv => dv.MaDV, (sdv, dv) => new { sdv.MaSD, dv.TenDV, sdv.ThanhTien })
                    .ToList();

                var list = new List<RecentTransaction>();

                // Map topups
                foreach (var t in recentTopups)
                {
                    list.Add(new RecentTransaction
                    {
                        EventType = "TopUp",
                        Id = null,
                        MaTK = t.MaTK,
                        Thoigian = t.ThoiGian,
                        TienNap = t.SoTien,
                        Services = 0m,
                        Account = db.TaiKhoanNguoiChois.Find(t.MaTK)?.TenDangNhap
                    });
                }

                // Map usages
                foreach (var u in recentUses)
                {
                    var svcList = services.Where(s => s.MaSD == u.MaSD).ToList();
                    decimal svcTotal = svcList.Sum(x => (decimal?)x.ThanhTien) ?? 0m;
                    list.Add(new RecentTransaction
                    {
                        EventType = "Usage",
                        Id = u.MaSD,
                        MaTK = u.MaTK,
                        Thoigian = u.ThoiGianBatDau,
                        TienNap = 0m,
                        Services = svcTotal,
                        Account = u.TaiKhoanNguoiChoi?.TenDangNhap
                    });
                }

                var combined = list.OrderByDescending(x => x.Thoigian).Take(count).ToList();

                return combined;
            }
        }
    }
}
