import { Component, Input, OnInit } from '@angular/core';
import { FeedMessage } from '../_entities/feed-message';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {

  @Input() messages: FeedMessage[] = [];
  
  constructor() { }

  ngOnInit(): void {

  }

}
