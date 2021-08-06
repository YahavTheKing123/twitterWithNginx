import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FeedModel } from '../_entities/feed-model';

const API_URL = 'http://localhost:5000/api/Feed/';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  getFeed(username:string): Observable<any> {
    return this.http.get<FeedModel>(API_URL + 'GetFeed/' + username);
  }

  getMyFeed(username:string): Observable<FeedModel> {
    return this.http.get<FeedModel>(API_URL + 'GetMyFeed/' + username);
  }

  postMessage(content: string): Observable<any> {
    return this.http.post(API_URL + 'PostMessage', {MessageBody:content});
  }
}