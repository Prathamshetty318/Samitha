using DubaiChaRaja.Models;

namespace DubaiChaRaja.Service
{
    public interface IFestivalService
    {
        void InsertFestivalRecords(string description, decimal amount, string type, int year, int userId);
        Task<IEnumerable<FestivalRecord>> GetByYearAsync(int year);
        Task<bool> DeleteFestivalRecordAsync(int id);
    }
}
