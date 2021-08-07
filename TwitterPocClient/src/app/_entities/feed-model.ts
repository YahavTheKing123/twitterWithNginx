import { FeedMessage } from './feed-message';

export interface FeedModel {
    messages:FeedMessage[];
    followees:string[];
}
