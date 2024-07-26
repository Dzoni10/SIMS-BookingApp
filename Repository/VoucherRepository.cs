using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        Repository<Voucher> voucherRepository;

        public VoucherRepository()
        {
            voucherRepository = new Repository<Voucher>();
        }

        public Voucher Save(Voucher voucher)
        {
            return voucherRepository.Save(voucher);
        }

        public void SaveAll(List<Voucher> vouchers)
        {
            voucherRepository.SaveAll(vouchers);
        }

        public void Update(Voucher voucher)
        {
            voucherRepository.Update(voucher);
        }

        public Voucher GetById(int id)
        {
            return voucherRepository.FindId(id);
        }

        public List<Voucher> GetAll()
        {
            return voucherRepository.GetAll();
        }
    }
}
