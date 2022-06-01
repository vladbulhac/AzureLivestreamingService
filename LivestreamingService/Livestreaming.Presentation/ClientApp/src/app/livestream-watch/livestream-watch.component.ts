import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { LivestreamEndpoints } from '../dtos/livestreamendpoints.dto';
import { LivestreamFullEndpoints } from '../dtos/livestreamfullendpoints.dto';
import { LivestreamingService } from '../services/livestreaming.service';

@Component({
  selector: 'app-livestream-watch',
  templateUrl: './livestream-watch.component.html',
  styleUrls: ['./livestream-watch.component.css']
})
export class LivestreamWatchComponent implements OnInit {
  private livestreamService: LivestreamingService;
  private route: ActivatedRoute;
  private router: Router;
  private livestreamId: string;
  public livestreamFullEndpoints: LivestreamFullEndpoints | null = null;

  public targetEndpoint: string | null;

  constructor(route: ActivatedRoute, router: Router, livestreamService: LivestreamingService) {
    this.targetEndpoint = null;
    this.livestreamId = "";
    this.route = route;
    this.router = router;
    this.livestreamService = livestreamService;
  }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.livestreamId = params['id'];
    });

    this.livestreamService.getLivestreamEndpoints(this.livestreamId).subscribe((result: LivestreamFullEndpoints) => {
      this.livestreamFullEndpoints = result;
      if (this.livestreamFullEndpoints!.playbackEndpoints && this.livestreamFullEndpoints!.playbackEndpoints.hlsManifest.length > 0)
        this.targetEndpoint = this.livestreamFullEndpoints!.playbackEndpoints.hlsManifest;
      else
        this.targetEndpoint = this.livestreamFullEndpoints!.runningEndpoints.hlsManifest;
      console.log(result);
    }, (error) => {
      console.log(error);
    });
  }

  redirectToStatusPage(id: string) {
    this.router.navigate(['/livestreams', id, 'status']);
  }
}
