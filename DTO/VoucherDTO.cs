using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class VoucherDTO : INotifyPropertyChanged
    {
        public int id;
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string name;

        public string Name
        {
            get
            {
                return "Voucher #" + id;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Voucher");
                }
            }
        }

        public DateTime expirationDate;
        public DateTime ExpirationDate
        {
            get
            {
                return expirationDate;
            }
            set
            {
                if(value != expirationDate)
                {
                    expirationDate = value;
                    OnPropertyChanged("ExpirationDate");
                }
            }
        }

        public VoucherStatus status;
        public VoucherStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                if (value != status)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public VoucherDTO()
        {
        }

        public VoucherDTO(Voucher voucher)
        {
            Id = voucher.Id;
            ExpirationDate = voucher.ExpirationDate;
            Status = voucher.Status;
        }

        public Voucher ToVoucher()
        {
            return new Voucher
            {
                Id = Id,
                ExpirationDate = ExpirationDate,
                Status = Status
            };
        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
