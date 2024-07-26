using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IVoucherRepository
    {
        public Voucher Save(Voucher voucher);
        public void SaveAll(List<Voucher> vouchers);
        public void Update(Voucher voucher);
        public Voucher GetById(int id);
        public List<Voucher> GetAll();
    }
}
