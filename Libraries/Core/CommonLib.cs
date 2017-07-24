using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    static public class CommonLib
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
    this Expression<Func<T, bool>> expr1,
    Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));
            if (expr1 == null)
            {
                return expr2;
            }
            else
            {
                var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
                var left = leftVisitor.Visit(expr1.Body);

                var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
                var right = rightVisitor.Visit(expr2.Body);

                return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
            }
        }

        private class ReplaceExpressionVisitor
        : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }

        public static void SendEmail(string from, string to, string subject, string body, string host, int port, string account, string pwd)
        {
            try
            {
                SmtpClient smtp = new SmtpClient(host, Convert.ToInt32(port));
                smtp.EnableSsl = true;
                smtp.Timeout = 10000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(account, pwd);

                MailMessage myMail = new MailMessage();
                myMail.From = new MailAddress(from);
                var tolist =  to.Split(';');
                foreach (var item in tolist)
                {
                    myMail.To.Add(new MailAddress(item));
                }
                myMail.Subject = subject;
                myMail.SubjectEncoding = Encoding.UTF8;
                myMail.Body = body;
                myMail.BodyEncoding = Encoding.UTF8;
                myMail.IsBodyHtml = true;

                smtp.Send(myMail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
