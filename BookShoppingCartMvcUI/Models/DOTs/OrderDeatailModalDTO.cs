 namespace BookShoppingCartMvcUI.Models.DOTs;

public class OrderDeatailModalDTO
{
    public string DivId { get; set; }
    public IEnumerable<OrderDetail> OrderDetail {  get; set; }
}
