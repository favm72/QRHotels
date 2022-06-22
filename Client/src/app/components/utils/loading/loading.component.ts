import { Component, OnDestroy, OnInit } from "@angular/core"
import { Subscription } from "rxjs"
import { NotificationService } from "src/app/services/notification.service"

@Component({
	selector: "app-loading",
	templateUrl: "./loading.component.html",
	styleUrls: ["./loading.component.css"],
})
export class LoadingComponent implements OnInit, OnDestroy {
	constructor(private notif: NotificationService) {}

	ngOnDestroy(): void {
		if (this.loadingSub) {
			this.loadingSub.unsubscribe()
		}
	}

	show: boolean = false
	loadingSub: Subscription = null

	ngOnInit(): void {
		this.loadingSub = this.notif.onLoading.subscribe(show => {
			this.show = show
		})
	}
}
