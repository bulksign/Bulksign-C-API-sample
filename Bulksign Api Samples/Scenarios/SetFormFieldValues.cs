﻿using System;
using System.IO;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class SetFormFieldValues
	{
		public void SendBundle()
		{
			AuthorizationApiModel token = new ApiKeys().GetAuthorizationToken();

			if (string.IsNullOrEmpty(token.UserToken))
			{
				Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
				return;
			}


			BulkSignApi api = new BulkSignApi();

			EnvelopeApiModel bundle = new EnvelopeApiModel();
			bundle.DaysUntilExpire = 10;
			bundle.Message = "Please sign this document";
			bundle.Subject = "Please Bulksign this document";
			bundle.Name = "Test bundle";

			bundle.Recipients = new[]
			{
				new RecipientApiModel()
				{
					Name = "Bulksign Test",
					Email = "contact@bulksign.com",
					Index = 1,
					RecipientType = RecipientTypeApi.Signer
				}
			};

			bundle.Documents = new[]
			{
				new DocumentApiModel()
				{
					Index = 2,
					FileName = "forms.pdf",
					FileContentByteArray = new FileContentByteArray()
					{
						ContentBytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\forms.pdf")
					},

					OverwriteValues = new []
					{
						new OverwriteFieldValueApiModel()
						{
							FieldName = "Text1",
							FieldValue = "This is a test text"
						},
						new OverwriteFieldValueApiModel()
						{
							FieldName = "Group3",
							FieldValue = "Choice2"
						}
					}
				}
			};


			BulksignResult<SendEnvelopeResultApiModel> result = api.SendEnvelope(token, bundle);

			if (result.IsSuccessful)
			{
				Console.WriteLine("Access code for recipient " + result.Response.RecipientAccess[0].RecipientEmail + " is " + result.Response.RecipientAccess[0].AccessCode);
				Console.WriteLine("Bundle id is : " + result.Response.EnvelopeId);
			}
			else
			{
				Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			}

		}
	}
}