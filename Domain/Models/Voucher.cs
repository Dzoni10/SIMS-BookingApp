using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public enum VoucherStatus { Valid=0, Used, Expired};
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public int GuideId {  get; set; }
        public int TouristId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public VoucherStatus Status { get; set; }

        public Voucher(int guideId, int touristId, DateTime expirationDate)
        {
            GuideId = guideId;
            TouristId = touristId;
            ExpirationDate = expirationDate;
            //Status = status;
            Status = VoucherStatus.Valid;
        }

        public Voucher() { }

        public Voucher(DateTime expirationDate, VoucherStatus status)
        {
            ExpirationDate = expirationDate;
            Status = status;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = Convert.ToInt32(values[1]);
            TouristId = Convert.ToInt32(values[2]);
            ExpirationDate = Convert.ToDateTime(values[3]);
            Status = (VoucherStatus)Enum.Parse(typeof(VoucherStatus), values[4]);
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                GuideId.ToString(),
                TouristId.ToString(),
                ExpirationDate.ToString(),
                Status.ToString()
            };
            return values;
        }

    }
}
