import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'test-ang';
  version: string;

  
  subscriptionName:string = "Oden.Security";
  uuid:string = "FXBOOKING_ODN01";
  topic:string = "Oden.Security";

  private _mainWin: fin.OpenFinWindow;

  constructor() {
    this.init();
  }

  init() {
    try {
      fin.desktop.main(() => this.initWithOpenFin());

    } catch (err) {
      this.initNoOpenFin();
    }
  };

  initWithOpenFin() {
    this._mainWin = fin.desktop.Window.getCurrent();

    fin.desktop.System.getVersion( (version)=> {
      try {
        this.version = "OpenFin version " + version;
        console.log("initialising subscription 1");   
        this.initInterApplicationBus();
      } catch (err) {
        console.error(err);   
      }
    });

  }

  public initInterApplicationBus():void {
    console.log("initialising subscription"); 
    try {  
    fin.desktop.InterApplicationBus.subscribe("*",this.topic,
    (msg, uuid, name)=>{
      console.log(msg);  
      console.log("initialising subscription");   
    });
  } catch (err) {
    console.error(err);   
  }
  }

  public initNoOpenFin():void {
    alert("OpenFin is not available - you are probably running in a browser.");
  }
}

