using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using TopUpApi.Models;

namespace TopUpApi.Service
{
    public class CyberSourceHttpClient : ICyberSourceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _secretKey;

        public CyberSourceHttpClient(string apiKey, string secretKey)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://apitest.cybersource.com/");

            _apiKey = apiKey;
            _secretKey = secretKey;
        }
        public async Task<PaymentResponse?> AuthorizeAmountAsync(string amount, string userId)
        {
            try
            {
                PaymentResponse payResponse = new PaymentResponse();
                if (string.IsNullOrWhiteSpace(_apiKey)
                    || string.IsNullOrWhiteSpace(_secretKey))
                {
                    return new PaymentResponse
                    {
                        ErrorInformation = new ErrorInformation
                        {

                            Message = "Please provide api and secret key to make transaction calls to the cybersource"
                        }
                    };
                }

                var requestUri = "pts/v2/payments";

                var requestModel = new
                {
                    clientReferenceInformation = new
                    {
                        code = "TC50171_3" //This could be any unique code which will represent the user initiating the request
                    },
                    //Dummy payment info but can be fetched from either DB or the UI
                    paymentInformation = new
                    {
                        card = new
                        {
                            number = "4111111111111111",
                            expirationMonth = "12",
                            expirationYear = "2031"
                        }
                    },
                    orderInformation = new
                    {
                        amountDetails = new
                        {
                            totalAmount = amount,
                            currency = "USD"
                        }
                    }
                };
                var jsonContent = JsonConvert.SerializeObject(requestModel);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var signature = GenerateHttpSignature(requestUri, jsonContent);


                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", signature);
                _httpClient.DefaultRequestHeaders.Add("v-c-merchant-id", _apiKey);

                var response = await _httpClient.PostAsync(requestUri, content);

                return JsonConvert.DeserializeObject<PaymentResponse>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> CaptureAmountAsync(string transactionId, string amount)
        {
            var requestUri = $"pts/v2/payments/{transactionId}/captures";

            var requestModel = new
            {
                clientReferenceInformation = new
                {
                    code = "TC50171_3"  //This could be any unique code which will represent the user initiating the request
                },
                //Dummy order info but can be fetched from either DB or the UI
                orderInformation = new
                {
                    amountDetails = new
                    {
                        totalAmount = amount,
                        currency = "USD"
                    }
                }
            };

            var jsonContent = JsonConvert.SerializeObject(requestModel);
            var response = await _httpClient.PostAsync(requestUri, new StringContent(jsonContent));

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        private string GenerateHttpSignature(string requestUri, string requestBody)
        {
            var message = $"(request-target): post {requestUri}\ndate: {DateTime.UtcNow.ToString("r")}\nhost: apitest.cybersource.com\ndigest: SHA-256={ComputeSha256Hash(requestBody)}";

            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            var signature = Convert.ToBase64String(signatureBytes);

            var authHeaderValue = $"Signature keyid=\"{_apiKey}\", algorithm=\"HmacSHA256\", headers=\"(request-target) date host digest\", signature=\"{signature}\"";

            return authHeaderValue;
        }

        private string ComputeSha256Hash(string input)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            var hash = Convert.ToBase64String(hashBytes);
            return hash;
        }
    }
}
