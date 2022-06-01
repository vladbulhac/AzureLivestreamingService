import { LivestreamDetails } from "./livestreamdetails.dto";
import { LivestreamEndpoints } from "./livestreamendpoints.dto";
import { PlaybackEndpoints } from "./playbackendpoints.dto";

export interface LivestreamFullEndpoints extends LivestreamDetails {
  runningEndpoints: LivestreamEndpoints;
  playbackEndpoints: PlaybackEndpoints;
}
