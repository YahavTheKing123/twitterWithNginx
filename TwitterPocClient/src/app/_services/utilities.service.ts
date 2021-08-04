import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UtilitiesService {

  constructor() { }

  getErrorMessage(err:any):string {
    console.log(err);
    if (err.status == 400 && err.error && err.error.errors)
    {
       var modelErrors = err.error.errors;
       var key = Object.keys(modelErrors)[0];
       var value = modelErrors[key];
       return key + ': ' + value;
    }
    else if (err.error.message)
    {
       return err.error.message;
    }
    else
    {
      return err.error.Message;
    }

  }
}
