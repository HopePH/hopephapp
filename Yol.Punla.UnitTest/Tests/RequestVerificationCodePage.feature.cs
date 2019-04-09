﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.3.2.0
//      SpecFlow Generator Version:2.3.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Yol.Punla.UnitTest.Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.2.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class RequestVerificationCodePageFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "RequestVerificationCodePage.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "RequestVerificationCodePage", "\tThis is the alternative workflow for signing in\r\n\twhich uses your email to recei" +
                    "ve a verification \r\n\tcode which you will confirm to sign in using your\r\n\talias", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "RequestVerificationCodePage")))
            {
                global::Yol.Punla.UnitTest.Tests.RequestVerificationCodePageFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(TestContext);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 7
#line 8
 testRunner.Given("I am not authenticated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
  testRunner.And("I am on the page \"WikiPage\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
  testRunner.And("the menu detail is closed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.When("I tap the hamburger icon", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 12
 testRunner.Then("I should see the menu detail is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
 testRunner.When("I tap the Write Down icon from the menu detail with authenticated \"false\" and sig" +
                    "ned up \"false\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
 testRunner.Then("I should see a message saying the user to sign up \"When logging in your profile w" +
                    "ill still remain anonymous to the other users.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 15
  testRunner.And("I am redirected to the page \"LogonPage\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
 testRunner.When("I tap the Alias link text in sign in with alias label", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
 testRunner.Then("I am redirected to the page \"RequestSigninVerificationCodePage\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 18
  testRunner.And("I should see that the email address field is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        public virtual void SigningInUsingVerficationCodeButEmailFieldIsNotSuppliedOrHasInvalidData(string email, string errorMsg, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Signing in using Verfication code but Email field is not supplied or has invalid " +
                    "data", exampleTags);
#line 20
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 21
 testRunner.Given("I am on the page \"RequestSigninVerificationCodePage\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 22
 testRunner.When(string.Format("I type my email \"{0}\" in the email field and tap the send verification code butto" +
                        "n", email), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.Then(string.Format("I should see an error message \"{0}\" in RequestSigninVerificationCodePage", errorMsg), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Signing in using Verfication code but Email field is not supplied or has invalid " +
            "data: momoyfailed.com")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RequestVerificationCodePage")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "momoyfailed.com")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:email", "momoyfailed.com")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:errorMsg", "Please enter a valid email address.")]
        public virtual void SigningInUsingVerficationCodeButEmailFieldIsNotSuppliedOrHasInvalidData_Momoyfailed_Com()
        {
#line 20
this.SigningInUsingVerficationCodeButEmailFieldIsNotSuppliedOrHasInvalidData("momoyfailed.com", "Please enter a valid email address.", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Signing in using Verfication code but Email field is not supplied or has invalid " +
            "data: ")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RequestVerificationCodePage")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:email", "")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:errorMsg", "Please enter a valid email address.")]
        public virtual void SigningInUsingVerficationCodeButEmailFieldIsNotSuppliedOrHasInvalidData_()
        {
#line 20
this.SigningInUsingVerficationCodeButEmailFieldIsNotSuppliedOrHasInvalidData("", "Please enter a valid email address.", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Signing in using Verfication code but Email field is not supplied or has invalid " +
            "data: momoy@gmail")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RequestVerificationCodePage")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "momoy@gmail")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:email", "momoy@gmail")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:errorMsg", "Please enter a valid email address.")]
        public virtual void SigningInUsingVerficationCodeButEmailFieldIsNotSuppliedOrHasInvalidData_MomoyGmail()
        {
#line 20
this.SigningInUsingVerficationCodeButEmailFieldIsNotSuppliedOrHasInvalidData("momoy@gmail", "Please enter a valid email address.", ((string[])(null)));
#line hidden
        }
        
        public virtual void SigningInUsingVerficationCodeByEnteringTheCorrectVerificationCodeSentToTheEmail(string email, string code, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Signing in using Verfication code by entering the correct verification code sent " +
                    "to the email", exampleTags);
#line 31
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 32
 testRunner.Given("I am on the page \"RequestSigninVerificationCodePage\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 33
  testRunner.And("the verification code is null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
 testRunner.When(string.Format("I type my email \"{0}\" in the email field and tap the send verification code butto" +
                        "n", email), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 35
 testRunner.Then("I am redirected to the page \"ConfirmVerificationCodePage\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 36
  testRunner.And(string.Format("I should receive a verification code \"{0}\" sent to my email", code), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
  testRunner.And("I should see that the verification code field is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
 testRunner.When(string.Format("I type the verification code \"{0}\" and tap the submit code button", code), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 39
 testRunner.Then("I am redirected to the page \"PostFeedPage\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Signing in using Verfication code by entering the correct verification code sent " +
            "to the email: alfeo.salano@gmail.com")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RequestVerificationCodePage")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "alfeo.salano@gmail.com")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:email", "alfeo.salano@gmail.com")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:code", "1111")]
        public virtual void SigningInUsingVerficationCodeByEnteringTheCorrectVerificationCodeSentToTheEmail_Alfeo_SalanoGmail_Com()
        {
#line 31
this.SigningInUsingVerficationCodeByEnteringTheCorrectVerificationCodeSentToTheEmail("alfeo.salano@gmail.com", "1111", ((string[])(null)));
#line hidden
        }
        
        public virtual void SigningInUsingVerficationCodeButVerificationIsIncorrectOrNotSupplied(string email, string code, string errorMessage, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Signing in using Verfication code but verification is incorrect or not supplied", exampleTags);
#line 45
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 46
 testRunner.Given("I am on the page \"RequestSigninVerificationCodePage\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 47
  testRunner.And("the verification code is null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.When(string.Format("I type my email \"{0}\" in the email field and tap the send verification code butto" +
                        "n", email), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 49
 testRunner.Then("I am redirected to the page \"ConfirmVerificationCodePage\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 50
  testRunner.And(string.Format("I should receive a verification code \"{0}\" sent to my email", code), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
  testRunner.And("I should see that the verification code field is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
 testRunner.When(string.Format("I type the verification code \"{0}\" and tap the submit code button", code), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 53
 testRunner.Then(string.Format("I should see an error message \"{0}\" in the ConfirmVerificationCodePage", errorMessage), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 54
  testRunner.And("I should stay on the same page \"ConfirmVerificationCodePage\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Signing in using Verfication code but verification is incorrect or not supplied: " +
            "Variant 0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RequestVerificationCodePage")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Variant 0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:email", "alfeo.salano@gmail.com")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:code", "1112")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:errorMessage", "Please enter a valid verification code")]
        public virtual void SigningInUsingVerficationCodeButVerificationIsIncorrectOrNotSupplied_Variant0()
        {
#line 45
this.SigningInUsingVerficationCodeButVerificationIsIncorrectOrNotSupplied("alfeo.salano@gmail.com", "1112", "Please enter a valid verification code", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Signing in using Verfication code but verification is incorrect or not supplied: " +
            "Variant 1")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RequestVerificationCodePage")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Variant 1")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:email", "alfeo.salano@gmail.com")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:code", "")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:errorMessage", "Please enter a valid verification code")]
        public virtual void SigningInUsingVerficationCodeButVerificationIsIncorrectOrNotSupplied_Variant1()
        {
#line 45
this.SigningInUsingVerficationCodeButVerificationIsIncorrectOrNotSupplied("alfeo.salano@gmail.com", "", "Please enter a valid verification code", ((string[])(null)));
#line hidden
        }
    }
}
#pragma warning restore
#endregion
