import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { LoaderComponent } from "./Core/interceptors/loader/loader.component";
import { LayoutComponent } from "./Shared/components/layout/layout.component";
import { NgIf } from '@angular/common';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoaderComponent, LayoutComponent,NgIf],
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'mogaERP.Client';
  isLoginRoute = false;
  constructor(private router: Router) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.isLoginRoute = event.urlAfterRedirects === '/login';
      });
  }
}
