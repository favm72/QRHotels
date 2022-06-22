import { Component, OnInit, ViewChild } from "@angular/core"
import { Router } from "@angular/router"
import { IntakeService } from "src/app/services/intake.service"
import { faEnvelope } from "@fortawesome/free-solid-svg-icons"
import { InfoService } from "src/app/services/info.service"
import { ModalComponent } from "../utils/modal/modal.component"

@Component({
	selector: "app-intake",
	templateUrl: "./intake.component.html",
	styleUrls: ["./intake.component.css"],
})
export class IntakeComponent implements OnInit {
	constructor(private intakeSrv: IntakeService, private router: Router, private info: InfoService) {}

	@ViewChild("mConfirm") mConfirm: ModalComponent
	iconMail = faEnvelope
	intakeList = []
	showAgree = false

	get totalAmount() {
		return this.intakeList.reduce((g, item) => {
			g[item.currency] = g[item.currency] ? g[item.currency] + item.total : item.total
			return g
		}, {})
	}

	convertDate(val: string) {
		return new Date(val)
	}

	async ngOnInit(): Promise<void> {
		this.showAgree = await this.intakeSrv.canAgree()
		this.intakeList = await this.intakeSrv.loadIntakeList()
	}

	goBack() {
		this.router.navigate(["master/main"])
	}

	goDetail(item) {
		this.intakeSrv.currentIntake = item
		this.router.navigate(["/intakedetail"])
	}

	goMail() {
		this.router.navigate(["/sendmail"])
	}

	openAgree() {
		this.mConfirm.open()
	}

	async agreeOrder() {
		await this.intakeSrv.saveAgreeOrder(() => {
			this.goBack()
		})
	}
}
