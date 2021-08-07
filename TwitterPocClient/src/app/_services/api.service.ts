import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FeedModel } from '../_entities/feed-model';

const API_FEED_URL = 'http://localhost:5000/api/Feed/';
const API_FOLLOW_UP_URL = 'http://localhost:5000/api/FollowUp/';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  getFeed(username:string): Observable<FeedModel> {
    return this.http.get<FeedModel>(API_FEED_URL + 'GetFeed/' + username);
  }

  getMyFeed(username:string): Observable<FeedModel> {
    return this.http.get<FeedModel>(API_FEED_URL + 'GetMyFeed/' + username);
  }

  postMessage(content: string): Observable<any> {
    return this.http.post(API_FEED_URL + 'PostMessage', {MessageBody:content});
  }

  followUser(username: string) : Observable<any> {
    return this.http.post(API_FOLLOW_UP_URL + 'Follow/' + username, {});
  }

  unfollowUser(username: string) : Observable<any> {
    return this.http.post(API_FOLLOW_UP_URL + 'Unfollow/' + username, {});
  }
}