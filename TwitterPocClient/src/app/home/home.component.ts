import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FeedMessage } from '../_entities/feed-message';
import { TokenStorageService } from '../_services/token-storage.service';
import { ApiService } from '../_services/api.service';
import { UtilitiesService } from '../_services/utilities.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  content:string = '';
  latestMessages:FeedMessage[] = [];
  notificationText:string ='';
  postMessageSuccess:boolean=false;

  constructor(private apiService: ApiService, private router:Router, private tokenStorage: TokenStorageService, private utilitiesService: UtilitiesService) { }

  ngOnInit(): void {

    var user = this.tokenStorage.getUser();
    if (!user)
    {
       this.router.navigate(['/register']);
       return;
    }
  }

  private showNotification(message:string, success:boolean):void{
    this.notificationText = message;
    this.postMessageSuccess = success;
    setTimeout(()=>{this.notificationText = ''}, 3000);
  }

  postMessage():void {
    /*

    this.messages.push({content: 'my message 2', username: 'some user'});
    this.showNotification('aaaa', true);
    */
   console.log('Message is going to posted: ' + this.content);
   this.apiService.postMessage(this.content).subscribe(
    data => {
      console.log(data);
      this.showNotification('Posted successfully!', false);

    },
    err => {
      const errorText = this.utilitiesService.getErrorMessage(err);
      this.showNotification(errorText, false);
    });
  }
  
}