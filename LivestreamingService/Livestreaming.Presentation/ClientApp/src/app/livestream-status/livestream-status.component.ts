import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { LivestreamDetails } from '../dtos/livestreamdetails.dto';
import { LivestreamEndpoints } from '../dtos/livestreamendpoints.dto';
import { StartLivestream } from '../dtos/startlivestream.dto';
import { StopLivestream } from '../dtos/stoplivestream.dto';
import { LivestreamingService } from '../services/livestreaming.service';
import { LoadingData } from '../utils/loading-data';

@Component({
  selector: 'app-livestream-status',
  templateUrl: './livestream-status.component.html',
  styleUrls: ['./livestream-status.component.css']
})
export class LivestreamStatusComponent implements OnInit {
  public livestreamData: LivestreamDetails | null;
  public livestreamEndpoints: LivestreamEndpoints | null;
  private livestreamService: LivestreamingService;
  private livestreamId: string;
  private route: ActivatedRoute;
  private router: Router;
  public message: string;
  public loadingAction: LoadingData;

  constructor(route: ActivatedRoute, router: Router, livestreamService: LivestreamingService) {
    this.route = route;
    this.router = router;
    this.message = "";
    this.livestreamId = "";
    this.livestreamData = null;
    this.livestreamEndpoints = null;
    this.livestreamService = livestreamService;
    this.loadingAction = new LoadingData();
  }

  ngOnInit(): void {
    this.loadingAction.genericRequest();

    this.route.params.subscribe((params: Params) => {
      this.livestreamId = params['id'];
    });

    this.livestreamService.getLivestreamStatus(this.livestreamId).subscribe((result: LivestreamDetails) => {
      this.loadingAction.loaded();
      this.livestreamData = result;
    }, (error) => {
      console.log(error);
      this.message = "Could not fetch this livestream's status.";
      this.loadingAction.loaded();
    });
  }

  watchVideo(id: string) {
    this.router.navigate(['/livestreams', id, 'watch']);
  }

  startLivestream(id: string) {
    this.loadingAction.startLivestreamRequest();

    const startData: StartLivestream = {
      livestreamId: id,
      userId: "user_id_placeholder"
    }
    this.livestreamService.startLivestream(startData).subscribe((result: LivestreamEndpoints) => {
      this.livestreamEndpoints = result;
      this.loadingAction.loaded();
    }, (error) => {
      console.log(error);
      this.loadingAction.loaded();
      this.message = "Could not start your livestream.";
    });
  }

  stopLivestream(id: string) {
    this.loadingAction.stopLivestreamRequest();
    const stopData: StopLivestream = {
      livestreamId: id,
      userId: "user_id_placeholder"
    };
    this.livestreamService.stopLivestream(stopData).subscribe((result) => {
      this.loadingAction.loaded();
      this.livestreamData!.status = "Saved";
    }, (error) => {
      this.loadingAction.loaded();
    });
  }
}
