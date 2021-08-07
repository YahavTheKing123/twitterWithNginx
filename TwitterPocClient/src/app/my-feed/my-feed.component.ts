import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FeedMessage } from '../_entities/feed-message';
import { ApiService } from '../_services/api.service';
import { TokenStorageService } from '../_services/token-storage.service';
import { UtilitiesService } from '../_services/utilities.service';

@Component({
  selector: 'app-my-feed',
  templateUrl: './my-feed.component.html',
  styleUrls: ['./my-feed.component.css']
})
export class MyFeedComponent implements OnInit {

  messages:FeedMessage[] = [];
  followees:string[] = [];
  username:string = '';
  errorText:string = '';

  constructor(private apiService: ApiService, private router:Router, private tokenStorage: TokenStorageService, private utilitiesService: UtilitiesService) { }

  ngOnInit(): void {

    var user = this.tokenStorage.getUser();
    if (!user)
    {
       this.router.navigate(['/register']);
       return;
    }
    this.getFeed();
  }

  private showError(message:string):void{
    this.errorText = message;
    setTimeout(()=>{this.errorText = ''}, 3000);
  }

  private debounceTimer:any;
  getDebounceFeed():void{
    const delay = 700;
    clearTimeout(this.debounceTimer);
    this.debounceTimer = setTimeout(()=>this.getFeed(), delay);
  }

  getFeed(): void {
    this.apiService.getMyFeed(this.username).subscribe(
      data => {
        this.messages = data.messages;
        this.followees = data.followees;
      },
      err => {
        this.showError(this.utilitiesService.getErrorMessage(err));
      }
    );
  }

  unfollowUser(username:string): void {
    this.apiService.unfollowUser(username).subscribe(
      data => {
        this.removeFollowee(username);
      },
      err => {
        this.showError(this.utilitiesService.getErrorMessage(err));
      }
    );

  }

  private removeFollowee(value:string) {
    
    for( var i = 0; i < this.followees.length; i++){ 
    
        if ( this.followees[i] === value) { 
    
          this.followees.splice(i, 1); 
        }
    }
  }

}
