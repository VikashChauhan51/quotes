import { Injectable } from '@angular/core';
import { AppConfiguration } from 'read-appsettings-json'
@Injectable({
  providedIn: 'root'
})
export class AppsettingService {

  constructor() {

    let serviceURL=AppConfiguration.Setting().Application.ServiceUrl;
    console.log(serviceURL);
  }
}
