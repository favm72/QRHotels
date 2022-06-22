import { Component, OnDestroy, OnInit } from "@angular/core"
import { Subscription } from "rxjs"
import { AlertModel } from "../../../models/notification"
import { NotificationService } from "../../../services/notification.service"
import { faBell, faCheckCircle, faExclamationTriangle } from "@fortawesome/free-solid-svg-icons"

@Component({
	selector: "app-alert",
	templateUrl: "./alert.component.html",
	styleUrls: ["./alert.component.css"],
})
export class AlertComponent implements OnInit, OnDestroy {
	model: AlertModel = new AlertModel()
	active: boolean = false
	customClass: any = {}
	alertSub: Subscription = null
	icon = faExclamationTriangle
	bell = faBell

	constructor(private notif: NotificationService) {}

	ngOnDestroy(): void {
		if (this.alertSub) {
			this.alertSub.unsubscribe()
		}
	}

	ngOnInit(): void {
		this.notif.onAlert.subscribe(am => {
			this.model = am
			if (am.type == "success") this.icon = faCheckCircle
			else this.icon = faExclamationTriangle

			this.active = true
		})
	}

	close() {
		this.active = false
	}
}
