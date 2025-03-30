import { Component } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';

@Component({
  selector: 'app-welcome',
  imports: [NgOptimizedImage],
  template: `
    <div class="w-100 min-h-100 bg-body-tertiary">
      <div class="container pt-lg-5 pt-md-2">
        <div class="bg-body py-lg-4 py-md-3 px-5 mx-auto">
          <div class="text-center mt-5">
            <img ngSrc="logo.png" width="369" height="92" alt="FacturX.NET Logo" />
          </div>

          <div class="d-flex justify-content-center gap-5 mt-2 mb-5">
            <a href="https://github.com/FacturX-NET/FacturXDotNet" class="hoverlink"><i class="bi bi-github"></i> github/FacturX-NET</a>
            <a href="https://www.nuget.org/profiles/FacturX.NET" class="hoverlink">
              <svg fill="currentColor" width="15" height="15" viewBox="0 0 24 24" role="img" xmlns="http://www.w3.org/2000/svg" class="mb-1">
                <title>NuGet icon</title>
                <path
                  d="M17.67 21.633a3.995 3.995 0 1 1 0-7.99 3.995 3.995 0 0 1 0 7.99m-7.969-9.157a2.497 2.497 0 1 1 0-4.994 2.497 2.497 0 0 1 0 4.994m8.145-7.795h-6.667a6.156 6.156 0 0 0-6.154 6.155v6.667a6.154 6.154 0 0 0 6.154 6.154h6.667A6.154 6.154 0 0 0 24 17.503v-6.667a6.155 6.155 0 0 0-6.154-6.155M3.995 2.339a1.998 1.998 0 1 1-3.996 0 1.998 1.998 0 0 1 3.996 0"
                />
              </svg>
              nuget/FacturX.NET
            </a>
          </div>
          <div class="text-center lead col-md-10 mb-5 m-auto">
            <p>Welcome to the FacturX.Net web application!</p>
            <p>
              This application allows you to easily access the features of the FacturX.Net library directly from your browser. You can create a FacturX document
              <a href="#" class="text-nowrap"> <i class="bi bi-file-earmark-pdf fs-4"></i> From an existing PDF file </a> by filling the structured data in a form,
              <a href="#" class="text-nowrap"> <i class="bi bi-file-earmark-code fs-4"> </i> From the invoice data </a> and an auto-generated PDF, or
              <a href="#" class="text-nowrap"> <i class="bi bi-plus-circle"></i> From an existing PDF and XML files </a>. You can
              <a href="#" class="text-nowrap"> <i class="bi bi-file-earmark-x fs-4"> </i> Edit an existing FacturX document </a>, including changing its structured data or its
              attachments. Finally you can <a href="#" class="text-nowrap"> <i class="bi bi-file-earmark-break"></i> Validate a FacturX document </a> against the the business rules
              defined by the specification.
            </p>
          </div>
          <hr class="w-25 m-auto" />
          <h4 class="d-flex justify-content-center mt-4 mb-4">This tool is also available as...</h4>
          <div class="row mx-xl-5 pb-4">
            <div class="text-center col-md-3 col-sm-6 py-2 position-relative">
              <h5>.NET library</h5>
              Available at <a href="https://www.nuget.org/packages/FacturXDotNet/">nuget.org</a>.
            </div>
            <div class="text-center col-md-3 col-sm-6 py-2">
              <h5>CLI tool</h5>
              Available as a .NET tool at <a href="https://www.nuget.org/packages/FacturXDotNet.CLI">nuget.org</a> or as a self-contained executable for all major platforms on
              <a href="https://github.com/FacturX-NET/FacturXDotNet/releases/latest">GitHub</a>.
            </div>
            <div class="text-center col-md-3 col-sm-6 py-2">
              <h5>Rest API</h5>
              Available <a href="#">for preview</a>, and as a self-hostable <a href="#">docker container</a>. This website uses the preview API!
            </div>
            <div class="text-center col-md-3 col-sm-6 py-2">
              <h5 class="text-body-tertiary">Desktop editor</h5>
              <span class="text-body-tertiary">Coming soon...</span>
            </div>
          </div>
        </div>

        <div class="mx-auto my-2 p-2">
          <div class="alert alert-light">
            <i class="bi bi-exclamation-triangle-fill text-danger"></i> <strong class="text-danger"> Do not share sensitive data </strong><br />
            This application (frontend and API) is hosted in a low-cost cloud environment. It is intended for demo purposes only and should be used solely for that purpose.
            Although I do not store your data, or use your it for any purpose other than the application, the hosting environment is beyond my control.
            <br />
            Please consider <a href="#">using the application locally</a> or <a href="#">self-hosting it</a> for your organization.
          </div>
        </div>

        <div class="mx-auto pb-4 px-2 py-5">
          <div class="d-flex justify-content-evenly">
            <div class="position-relative col-3">
              <h5><i class="bi bi-envelope-at"></i> Contact</h5>
              <p>
                Have questions about FacturX.NET?<br />
                <a href="mailto:contact@facturxdotnet.org" class="stretched-link">Drop me a message</a>.
              </p>
            </div>
            <div class="position-relative col-3">
              <h5><i class="bi bi-chat-dots"></i> Issue</h5>
              <p>
                Encountered a bug or unexpected behavior?<br />
                Report it by <a href="https://github.com/FacturX-NET/FacturXDotNet" class="stretched-link">opening an issue</a>.
              </p>
            </div>
            <div class="position-relative col-3">
              <h5><i class="bi bi-star"></i> Support</h5>
              <p>
                Enjoying FacturX.NET?<br />
                Show your support by <a href="https://github.com/FacturX-NET/FacturXDotNet" class="stretched-link">starring the project</a>.
              </p>
            </div>
          </div>
          <div class="p-2 pt-5 text-center">
            <strong>© 2025 Ismail Bennani</strong>, with <i class="bi-heart-fill"></i> and <i class="bi bi-cup-hot-fill"></i><br />
            The tools are open source and under the MIT License, feel free to use, modify, and share.
          </div>
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class WelcomePage {}
