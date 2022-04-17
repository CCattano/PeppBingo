import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

document.body.classList.add('bg-dark', 'text-light');

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));

// If needed later
// "@fortawesome/free-regular-svg-icons": "^5.15.3"
