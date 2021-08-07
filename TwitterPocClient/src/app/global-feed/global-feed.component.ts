import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FeedMessage } from '../_entities/feed-message';
import { ApiService } from '../_services/api.service';
import { TokenStorageService } from '../_services/token-storage.service';
import { UtilitiesService } from '../_services/utilities.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-global-feed',
  templateUrl: './global-feed.component.html',
  styleUrls: ['./global-feed.component.css']
})
export class GlobalFeedComponent implements OnInit {

   messages:FeedMessage[] = [];
   username:string = '';
   errorText:string = '';
   suggestedUsers:string[] = [];

  constructor(private apiService: ApiService, private router:Router, private tokenStorage: TokenStorageService, private utilitiesService: UtilitiesService) { }

  ngOnInit(): void {

    var user = this.tokenStorage.getUser();
    if (!user)
    {
       this.router.navigate(['/register']);
       return;
    }
    else
    {
      this.getFeed();
    }
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
    const username = this.username;
    console.log('Sending getFeed request: username=' + username);
    this.apiService.getFeed(username).subscribe(
      data => {
        this.messages = data.messages;
        this.suggestedUsers = this.messages.map(m=>m.username).filter((value, index, self)=>self.indexOf(value) === index);
      },
      err => {
        this.showError(this.utilitiesService.getErrorMessage(err));
      }
    );
  }

  followUser(username:string): void {
    this.apiService.followUser(username).subscribe(
      data => {
        this.removeSuggestedUser(username);
      },
      err => {
        this.showError(this.utilitiesService.getErrorMessage(err));
      }
    );

  }

  removeSuggestedUser(value:string) {
    
    for( var i = 0; i < this.suggestedUsers.length; i++){ 
    
        if ( this.suggestedUsers[i] === value) { 
    
          this.suggestedUsers.splice(i, 1); 
        }
    
    }
  }

}
