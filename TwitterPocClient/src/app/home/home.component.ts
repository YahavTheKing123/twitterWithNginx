import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FeedMessage } from '../_entities/feed-message';
import { TokenStorageService } from '../_services/token-storage.service';
import { UserService } from '../_services/user.service';
import { UtilitiesService } from '../_services/utilities.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  content:string = '';
  messages:FeedMessage[] = [{content: 'my message', username: 'some user'}];
  alertText:string ='';
  postMessageSuccess:boolean=false;

  constructor(private userService: UserService, private router:Router, private tokenStorage: TokenStorageService, private utilitiesService: UtilitiesService) { }

  ngOnInit(): void {

    var user = this.tokenStorage.getUser();
    if (!user)
    {
       this.router.navigate(['/register']);
       return;
    }
    this.content = 'sddsds';

    /*
    this.userService.getFeed().subscribe(
      data => {
        this.content = JSON.stringify(data);
      },
      err => {
        this.content = this.utilitiesService.getErrorMessage(err);
      }
    );
    */
  }

  showNotification(message:string, success:boolean):void{
    this.alertText = message;
    this.postMessageSuccess = success;
    setTimeout(()=>{this.alertText = ''}, 3000);
  }

  postMessage():void {
    console.log(this.content);
    this.messages.push({content: 'my message 2', username: 'some user'});
    this.showNotification('aaaa', true);
  }
}