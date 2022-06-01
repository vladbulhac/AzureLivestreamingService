import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SetupLivestream } from '../dtos/setuplivestream.dto';
import { LivestreamingService } from '../services/livestreaming.service';
import { SetupLivestreamResult } from '../dtos/setuplivestreamresult.dto';
import { LoadingData } from '../utils/loading-data';

@Component({
  selector: 'app-livestream-setup',
  templateUrl: './livestream-setup.component.html',
  styleUrls: ['./livestream-setup.component.css']
})
export class LivestreamSetupComponent implements OnInit {
  public loadingAction: LoadingData;
  public setupForm!: FormGroup;
  public setupResult!: SetupLivestreamResult | null;
  private livestreamService: LivestreamingService;

  constructor(livestreamService: LivestreamingService) {
    this.livestreamService = livestreamService;
    this.setupResult = null;
    this.loadingAction = new LoadingData();
  }

  ngOnInit(): void {
    this.setupForm = new FormGroup({
      'title': new FormControl(null, [Validators.required]),
      'description': new FormControl(null, []),
      'recording-duration': new FormControl(null, [Validators.required, Validators.min(1), Validators.max(25)]),
      'streaming-protocol': new FormControl(null, [Validators.required]),
      'encoding-type': new FormControl(null, [Validators.required])
    });

    this.loadingAction.loaded();
  }

  onSubmit(): void {
    this.loadingAction.setupLivestreamRequest();

    const setupData: SetupLivestream = {
      userId: "user_id_placeholder",
      title: this.setupForm.value['title'],
      description: this.setupForm.value['description'],
      recordingDuration: this.setupForm.value['recording-duration'],
      streamingProtocol: this.setupForm.value['streaming-protocol'],
      encodingType: this.setupForm.value['encoding-type']
    }

    this.livestreamService.setupLivestream(setupData).subscribe((response: SetupLivestreamResult) => {
      this.setupResult = response;
      this.loadingAction.loaded();
    }, (error) => {
      console.log(error);
      this.loadingAction.loaded();
    });
  }
}
