using MimeKit;
using Newtonsoft.Json;
using VSRAdminAPI.Model.Common;
using VSRAdminAPI.Model;
using VSRAdminAPI.Repository;
using VSRAdminAPI.Services;
using MailKit.Net.Smtp;
using System.Dynamic;

public class CompanyService : ICompanyService
{
    private readonly IConfiguration _configuration;
    private readonly ICompanyRepository _companyRepository;
    private readonly ILogger<CompanyService> _logger;

    public CompanyService(
        IConfiguration configuration,
        ICompanyRepository companyRepository,
        ILogger<CompanyService> logger)
    {
        _configuration = configuration;
        _companyRepository = companyRepository;
        _logger = logger;
    }

    public GenericResponse ValidateLogin(LoginValues loginValues)
    {
        List<LoginResultset> loginResultsets = new List<LoginResultset>();
        dynamic dynamic = new ExpandoObject();
        GenericResponse genericResponse = new GenericResponse();
        loginResultsets = _companyRepository.ValidateLogin(loginValues);
        dynamic.data = loginResultsets;
        genericResponse.Data = dynamic;
        return genericResponse;
    }
    public GenericResponse AddCompany(CustomerFileData addCustomerFormData)
    {
        GenericResponse genericResponse = new GenericResponse();
        dynamic dynamic = new ExpandoObject();
        dynamic dbConfig = _configuration.GetSection("connectionStrings");
        //CompanyRepository cmr = new CompanyRepository();
        string URL = string.Empty;
        string filePath = dbConfig["restaurantlogoPath"];


        var image = addCustomerFormData.Filename;

        // MasterCustomer objContact = new MasterCustomer();



        var contactData = Convert.ToString(addCustomerFormData.Customerdata);
        var objContact = JsonConvert.DeserializeObject<MasterCustomer>(contactData);

        // Save image if provided
        if (image != null)
        {
            URL = objContact.DID;
            filePath = Path.Combine(filePath, $"{URL}.jpg");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            image.CopyTo(fileStream);
        }

        List<Defaultresultset> defaultresultset = _companyRepository.AddCompany(objContact, ref dbConfig);

        if (Convert.ToInt32(defaultresultset.Count) > 0 && defaultresultset[0].Status > 0)
        {
            if (objContact.Type == 1 || objContact.Type == 2)
            {
                string _fromName = dbConfig.GetSection("MailFromName").Value;
                string _fromAddress = dbConfig.GetSection("MailFromAddress").Value;
                string _Password = dbConfig.GetSection("FromPass").Value;
                string _ToAddress = objContact.Email ?? "";
                string _ToName = objContact.ContactPerson ?? "";
                string _Subject = dbConfig.GetSection("CreateSubject").Value;
                string _TextBody = "Login details:";
                string _TextLink = dbConfig.GetSection("CreateLink").Value; ;
                string _TextPassword = dbConfig.GetSection("CreatePassword").Value;
                string _Welcomegreeting = dbConfig.GetSection("Welcomegreeting").Value;

                string _HtmlBody = "<div style='box-shadow:0px 2px 3px #ddd;padding:3px 3px 10px 0px;margin:10px auto;width:800px;font-family: serif;background:rgba(238, 238, 238, 0.59);'>"
                                    +
                                    "<br/>"
                                    +
                                    "<h3> &nbsp;&nbsp;Hi&nbsp;"
                                    +
                                   objContact.ContactPerson
                                    +
                                    ",<br/><br/> &nbsp;&nbsp;Thanks for signing up with " + _Welcomegreeting + "!"
                                    +
                                    " <br/><br/> &nbsp;&nbsp;Here are the details for your dashboard,"
                                    +
                                    " <br/><br/> &nbsp;&nbsp;Link :" + _TextLink
                                    +
                                    "<br/> &nbsp;&nbsp;Username :" + objContact.Username
                                    +
                                    "<br/> &nbsp;&nbsp;Password :" + _TextPassword
                                    +
                                    "<br/><br/> &nbsp;&nbsp;As always, our support team can be reached at[sales@voicesnap.com] if you ever get stuck."
                                    +
                                    "<br/><br/> &nbsp;&nbsp;Have a great day!"
                                    +
                                    "</h3></div>";

                bool boolval = Send_SMTP_Mail(_fromAddress, _fromName, _Password, _ToName, _ToAddress, _Subject, _HtmlBody, _TextBody);

                if (boolval == true)
                {
                    defaultresultset[0].Status = 1;
                    defaultresultset[0].Message = defaultresultset[0].Message + " & Mail Sent to client";
                    dynamic.resultSet = defaultresultset;
                    genericResponse.Data = dynamic;
                }
                else
                {
                    defaultresultset[0].Status = 2;
                    defaultresultset[0].Message = defaultresultset[0].Message + " & Mail not Sent to client. Please check email id";
                    dynamic.resultSet = defaultresultset;
                    genericResponse.Data = dynamic;

                }
            }
            else
            {
                dynamic.resultSet = defaultresultset;
                genericResponse.Data = dynamic;
            }
        }
        else
        {
            dynamic.resultSet = defaultresultset;
            genericResponse.Data = dynamic;
        }

        return genericResponse;
    }
    public GenericResponse LoadCompany(CompanySearch loadcompanyInput)
    {
        GenericResponse genericResponse = new GenericResponse();
        dynamic dynamicResult = new ExpandoObject();

        int totalrow = 0;
        List<CompanyInfo> defaultresultset = _companyRepository.LoadCompany(loadcompanyInput, ref totalrow);

        dynamicResult.totalrow = totalrow;
        dynamicResult.resultset = defaultresultset;

        genericResponse.Data = dynamicResult; // Assign dynamicResult to Data
        return genericResponse;
    }

    public Boolean Send_SMTP_Mail(string _fromAddress, string _fromName, string _Password, string _ToName, string _ToAddress, string _Subject, string _HtmlBody, string _TextBody)
    {
        try
        {
            MimeMessage message = new MimeMessage();
            BodyBuilder bodyBuilder = new BodyBuilder();

            MailboxAddress from = new MailboxAddress(_fromName, _fromAddress);
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(_ToName, _ToAddress);
            message.To.Add(to);

            message.Subject = _Subject;

            bodyBuilder.HtmlBody = _HtmlBody;

            bodyBuilder.TextBody = _TextBody;

            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(_fromAddress, _Password);

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
}
