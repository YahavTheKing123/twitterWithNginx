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
  disableButton:boolean = false;

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
   this.disableButton = true;
   const content = this.content;
   console.log(content.length);
   console.log('Message is going to posted: ' + content);
   this.apiService.postMessage(content).subscribe(
    data => {
      console.log(data);
      this.showNotification('Posted successfully!', true);
      this.latestMessages.push({username:'Me', content:content, time:new Date()});
      this.content = '';
      this.disableButton = false;
    },
    err => {
      const errorText = this.utilitiesService.getErrorMessage(err);
      this.showNotification(errorText, false);
      this.disableButton = false;
    });
  }

}