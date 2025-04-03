import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AboutLicensesComponent } from './about-licenses.component';

@Component({
  selector: 'app-about',
  template: `
    <div class="h-100 container pt-2">
      <div class="py-4">
        <a routerLink="/editor"><i class="bi bi-chevron-left"></i> Back to editor</a>
      </div>
      <div class="h-100 row">
        <div class="col-3 d-none d-md-block"></div>
        <div class="h-100 col">
          <h2>About FacturX.NET</h2>

          <p>Work with FacturX documents in .NET</p>

          <div>
            <span class="fw-bold">Links</span>
            <ul>
              <li>Github: <a href="https://github.com/FacturX-NET/FacturXDotNet">https://github.com/FacturX-NET/FacturXDotNet</a></li>
              <li>Nuget: <a href="https://www.nuget.org/packages/FacturXDotNet/">https://www.nuget.org/packages/FacturXDotNet/</a></li>
            </ul>
          </div>

          <p><span class="fw-bold">License</span>: The tools are open source and under the <span class="fw-bold">MIT</span> License, feel free to use, modify, and share.</p>

          <p>Copyright © 2025 Ismail Bennani</p>

          <h2>Web Editor</h2>

          <p>
            The web editor is a demo application that allows you to create and edit FacturX documents. It is built using Angular and TypeScript. It is also open-source and under
            the <span class="fw-bold">MIT</span> license, and available
            <a href="https://github.com/FacturX-NET/FacturXDotNet/tree/main/src/Tests.FacturXDotNet.WebEditor">on GitHub</a>.
          </p>

          <h3>Dependencies</h3>
          <app-about-licenses></app-about-licenses>
        </div>
      </div>
    </div>
  `,
  imports: [RouterLink, AboutLicensesComponent],
})
export class AboutPage {}
