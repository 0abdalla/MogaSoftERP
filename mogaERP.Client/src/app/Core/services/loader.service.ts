import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  private requests = 0;
  loading = signal(false);

  show() {
    this.requests++;
    this.loading.set(true);
  }

  hide() {
    this.requests--;
    if (this.requests <= 0) {
      this.requests = 0;
      this.loading.set(false);
    }
  }
}
