import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const API_URL = 'http://localhost:5000/api/Feed/';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }



  getFeed(): Observable<any> {
    return this.http.get(API_URL + 'GetFeed');
  }

  postMessage(content: string): Observable<any> {
    return this.http.post(API_URL + 'PostMessage', {MessageBody:content});
  }
}