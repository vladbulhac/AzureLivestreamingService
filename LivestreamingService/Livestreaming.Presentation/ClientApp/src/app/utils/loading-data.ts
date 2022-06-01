export class LoadingData {
  public isLoading: boolean;
  private messages: string[];
  public message: string;

  constructor() {
    this.isLoading = true;
    this.messages = [
      "Fetching data, please wait...",
      "Allocating resources for your livestream, this might take a while...",
      "Starting your livestream, this might take a while...",
      "Releasing the resources allocated for this livestream, this might take a while..."
    ];
    this.message = this.messages[0];
  }

  private loading() {
    this.isLoading = true;
  }
  public loaded() {
    this.isLoading = false;
  }
  public startLivestreamRequest() {
    this.loading();
    this.message = this.messages[2];
  }
  public setupLivestreamRequest() {
    this.loading();
    this.message = this.messages[1];
  }
  public stopLivestreamRequest() {
    this.loading();
    this.message = this.messages[3];
  }
  public genericRequest() {
    this.loading();
    this.message = this.messages[0];
  }
}
