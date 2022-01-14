import { Component, ElementRef, ViewChild } from '@angular/core';
import WebViewer from '@pdftron/webviewer';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent {

	@ViewChild('viewer') viewerRef!: ElementRef;

	ngAfterViewInit(): void {
		WebViewer({
			licenseKey: 'INSERT_LICENCE_HERE',
			path: '../lib',
			preloadWorker: 'pdf',
		}, this.viewerRef.nativeElement)
			.then(instance => {
				instance.UI.loadDocument('http://localhost:5000/', {
					customHeaders: {
						"X-API-Key": `asdf1234`
					},
					withCredentials: true
				})
			});
	}
}
