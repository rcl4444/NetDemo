using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Serialization;

namespace AEOWebapi.Controllers
{
    //[Authorize]
    public class AccountController: ApiController
    {
        public HttpResponseMessage Get()
        {
            //JSON默认序列化方式会导致文本输出""包含,这里需要自己移除headers
            return new HttpResponseMessage()
            {
                Content = new StringContent("<xml></xml>", System.Text.Encoding.UTF8, "text/plain")
            };
        }

        public HttpResponseMessage Post(HipacPush require)
        {
            StringBuilder buffer = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(require.GetType());
            using (TextWriter writer = new StringWriter(buffer))
            {
                serializer.Serialize(writer, require);
            }

            return new HttpResponseMessage() {
                Content = new StringContent(buffer.ToString())
            };
        }
    }

    [XmlRoot(ElementName = "HipacPush")]
    public class HipacPush
    {
        /// <summary>
        /// 请求头
        /// </summary>
        [XmlElement(ElementName = "Head")]
        public Head Head { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        [XmlElement(ElementName = "Body")]
        public Body Body { get; set; }
    }

    /// <summary>
    /// 头信息
    /// </summary>
    [XmlRoot(ElementName = "Head")]
    public class Head
    {
        /// <summary>
        /// 版本 默认为1.0 
        /// </summary>
        [XmlElement(ElementName = "version")]
        public string version { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        [XmlElement(ElementName = "service")]
        public string service { get; set; }

        /// <summary>
        /// 推送报文
        /// </summary>
        [XmlElement(ElementName = "sendID")]
        public string sendID { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        [XmlElement(ElementName = "appKey")]
        public string appKey { get; set; }


        /// <summary>
        /// 签名的串
        /// </summary>
        [XmlElement(ElementName = "sign")]
        public string sign { get; set; }
    }

    /// <summary>
    /// 内容体
    /// </summary>
    [XmlRoot(ElementName = "Body")]
    public class Body
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        [XmlElement(ElementName = "Order")]
        public Order Order { get; set; }

        /// <summary>
        /// 支付信息
        /// </summary>
        [XmlElement(ElementName = "PayInfo")]
        public PayInfo PayInfo { get; set; }

        /// <summary>
        /// 消费者信息
        /// </summary>
        [XmlElement(ElementName = "Customer")]
        public Customer Customer { get; set; }

        /// <summary>
        /// 商品集合
        /// </summary>
        [XmlElement(ElementName = "OrderItemList")]
        public List<OrderItem> OrderItemList { get; set; }
    }


    ///// <summary>
    ///// 商品集合
    ///// </summary>
    //[XmlRoot(ElementName = "OrderItemList")]
    //public class OrderItemList
    //{
    //    /// <summary>
    //    /// 商品集合
    //    /// </summary>
    //    [XmlElement(ElementName = "OrderItem")]
    //    public List<OrderItem> OrderItem { get; set; }

    //}

    /// <summary>
    /// 单个商品信息
    /// </summary>
    [XmlRoot(ElementName = "OrderItem")]
    public class OrderItem
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        [XmlElement(ElementName = "itemName")]
        public string itemName { get; set; }

        /// <summary>
        /// 商品货号  
        /// </summary>
        [XmlElement(ElementName = "itemSupplyNo")]
        public string itemSupplyNo { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [XmlElement(ElementName = "itemQuantity")]
        public int itemQuantity { get; set; }

        /// <summary>
        /// 增值税
        /// </summary>
        [XmlElement(ElementName = "addTaxRate")]
        public decimal addTaxRate { get; set; }

        /// <summary>
        /// 消费税
        /// </summary>
        [XmlElement(ElementName = "exciseRate")]
        public decimal exciseRate { get; set; }

        /// <summary>
        /// 关税
        /// </summary>
        [XmlElement(ElementName = "tariffRate")]
        public decimal tariffRate { get; set; }

        /// <summary>
        /// 规格名称
        /// </summary>
        [XmlElement(ElementName = "specNme")]
        public string specNme { get; set; }

        /// <summary>
        /// 规格数
        /// </summary>
        [XmlElement(ElementName = "specNum")]
        public int specNum { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        [XmlElement(ElementName = "itemPrice")]
        public decimal itemPrice { get; set; }

        /// <summary>
        /// 商品总价
        /// </summary>
        [XmlElement(ElementName = "itemTotal")]
        public decimal itemTotal { get; set; }

        /// <summary>
        /// 总税款
        /// </summary>
        [XmlElement(ElementName = "itemTotalTax")]
        public string itemTotalTax { get; set; }
    }

    /// <summary>
    /// 订单信息
    /// </summary>
    [XmlRoot(ElementName = "Order")]
    public class Order
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        [XmlElement(ElementName = "orderNum")]
        public string orderNum { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        [XmlElement(ElementName = "orderDate")]
        public string orderDate { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        [XmlElement(ElementName = "totalOrderAmount")]
        public decimal totalOrderAmount { get; set; }

        /// <summary>
        /// 订单总税额
        /// </summary>
        [XmlElement(ElementName = "totalTaxAmount")]
        public decimal totalTaxAmount { get; set; }

        /// <summary>
        /// 订单支付金额
        /// </summary>
        [XmlElement(ElementName = "totalPayAmount")]
        public decimal totalPayAmount { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        [XmlElement(ElementName = "logisticsAmount")]
        public decimal logisticsAmount { get; set; }
    }

    /// <summary>
    /// 支付信息
    /// </summary>
    [XmlRoot(ElementName = "PayInfo")]
    public class PayInfo
    {
        /// <summary>
        /// 支付交易号
        /// </summary>
        [XmlElement(ElementName = "payNo")]
        public string payNo { get; set; }

        /// <summary>
        /// 支付类型（微信支付，支付宝，盛付通）
        /// </summary>
        [XmlElement(ElementName = "payType")]
        public string payType { get; set; }

        /// <summary>
        /// 支付时间（格式：YYYYMMDD hh:mm:ss）
        /// </summary>
        [XmlElement(ElementName = "payTime")]
        public string payTime { get; set; }

        /// <summary>
        /// 支付公司名称
        /// </summary>
        [XmlElement(ElementName = "payCompanyName")]
        public string payCompanyName { get; set; }
    }


    /// <summary>
    /// 消费者信息
    /// </summary>
    [XmlRoot(ElementName = "Customer")]
    public class Customer
    {
        /// <summary>
        /// 消费者姓名
        /// </summary>
        [XmlElement(ElementName = "custName")]
        public string custName { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [XmlElement(ElementName = "custIdNum")]
        public string custIdNum { get; set; }

        /// <summary>
        /// 消费者手机号码
        /// </summary>
        [XmlElement(ElementName = "custPhone")]
        public string custPhone { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [XmlElement(ElementName = "custProvice")]
        public string custProvice { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [XmlElement(ElementName = "custCity")]
        public string custCity { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [XmlElement(ElementName = "custArea")]
        public string custArea { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [XmlElement(ElementName = "custAddress")]
        public string custAddress { get; set; }
    }
}
