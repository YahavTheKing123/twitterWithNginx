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

  }

  private showError(message:string):void{
    this.errorText = message;
    setTimeout(()=>{this.errorText = ''}, 3000);
  }

  getFeed(): void {
    this.apiService.getMyFeed(this.username).subscribe(
      data => {
        this.messages = data.messages;
      },
      err => {
        this.showError(this.utilitiesService.getErrorMessage(err));
      }
    );
  }

}
