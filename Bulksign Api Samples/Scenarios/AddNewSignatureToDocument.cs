﻿using Bulksign.Api;
using System;
using System.IO;
using Bulksign_Api_Samples;

namespace Bulksign.ApiSamples
{
    public class AddNewSignatureToDocument
    {

        public void SendBundle()
        {

            BulkSignApi api = new BulkSignApi();

            BulksignBundle bb = new BulksignBundle();
            bb.DaysUntilExpire = 10;
            bb.DisableNotifications = false;
            bb.NotificationOptions = new BulksignNotificationOptions();

            BulksignRecipient recipient = new BulksignRecipient();
            recipient.Name = "Bulksign Test";
            recipient.Email = "contact@bulksign.com";
            recipient.Index = 1;
            recipient.RecipientType = BulksignApiRecipientType.Signer;

            BulksignDocument document = new BulksignDocument();
            document.Index = 1;
            document.FileName = "singlepage.pdf";

            //add new signature
            document.NewSignatures = new BulksignNewSignature[1];

            //width,height, left and top values are in pixels
            var newSig = new BulksignNewSignature();
            newSig.Height = 100;
            newSig.Width = 250;
            newSig.PageIndex = 1;
            newSig.Left = 100;
            newSig.Top = 500;
            //assign the signature field to the recipient. The assignment is done by the email address
            newSig.MappingIdentifier = recipient.Email;

            document.NewSignatures[0] = newSig;

            document.ContentBytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\singlepage.pdf");
            bb.Documents = new[]
            {
                document
            };
            

            BulksignAuthorization token = new ApiKeys().GetAuthorizationToken();

            if (string.IsNullOrEmpty(token.UserToken))
            {
                Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
                return;
            }


            BulksignResult<BulksignSendBundleResult> result = api.SendBundle(token, bb);

            Console.WriteLine("Api call is successfull: " + result.IsSuccessful);

            if (result.IsSuccessful)
            {
                Console.WriteLine("Access code for recipient " + result.Response.AccessCodes[0].RecipientName + " is " + result.Response.AccessCodes[0].AccessCode);
                Console.WriteLine("Bundle id is : " + result.Response.BundleId);
            }

        }
    }
}